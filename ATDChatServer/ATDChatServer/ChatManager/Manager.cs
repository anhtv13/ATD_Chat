using ATDChatDefinition;
using ATDChatServer.Adapter;
using ATDChatServer.ChatManager;
using Core.Controller;
using ServerModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer
{
    public class Manager
    {
        private static Manager m_Instance;
        public static Manager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Manager();
                return m_Instance;
            }
        }

        //connectionId, customerId
        private Dictionary<string, string> m_mapConnectionIdCustomer = new Dictionary<string, string>();

        //customerId/customerInfo
        private ConcurrentDictionary<string, CustomerInfo> m_customerInfoDict = new ConcurrentDictionary<string, CustomerInfo>();

        //groupId/groupInfo
        private ConcurrentDictionary<string, GroupInfo> m_groupInfoDict = new ConcurrentDictionary<string, GroupInfo>();

        //id, object Addfriendrequest
        private ConcurrentDictionary<string, AddFriendRequest> m_AddFriendRequestDict = new ConcurrentDictionary<string, AddFriendRequest>();

        //id, object InviteGroupRequest
        private ConcurrentDictionary<string, InviteGroupRequest> m_InviteGroupRequestDict = new ConcurrentDictionary<string, InviteGroupRequest>();

        /**
        /// <summary>
        /// cache message to view history
        /// receiverId, messageInfo
        /// </summary>
        private Dictionary<string, List<MessageInfo>> m_messageDict = new Dictionary<string, List<MessageInfo>>();

        /// <summary>
        /// cache offline message to get when logging in
        /// receiverId, List<MessageInfo>
        /// </summary>
        private Dictionary<string, List<MessageInfo>> m_offlineMessageDict = new Dictionary<string, List<MessageInfo>>();
        */

        //database
        private PersistanceAdapter m_adapter = new PersistanceAdapter();

        //generate messageId
        private long m_messageId = DateTime.Now.Ticks;

        //lock
        private object m_lock = new object();

        private Manager()
        {
        }

        public void StopAdapter()
        {
            m_adapter.StopAdapter();
        }

        #region ConnectionId
        public bool ConnectionIdAuthenticated(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
                return true;
            return false;
        }

        public void AddConnectionId(string customerId, string connectionId)
        {
            if (!m_mapConnectionIdCustomer.ContainsKey(connectionId))
                m_mapConnectionIdCustomer.Add(connectionId, customerId);

            CustomerInfo customer = GetCustomerByCustomerId(customerId);
            if (customer == null)
            {
                //if data not exist
                CustomerInfo c = new CustomerInfo(customerId, customerId, ChatStatus.Available);
                c.Role = CustomerRole.User;
                m_customerInfoDict.TryAdd(customerId, c);
                m_adapter.AddCustomer(c);//add new to DB
            }
            m_customerInfoDict[customerId].AddNewConnectionID(connectionId);
        }

        public void RemoveConnectionId(string connectionId)
        {
            m_mapConnectionIdCustomer.Remove(connectionId);
        }
        #endregion

        #region Friend
        /// <summary>
        /// trong trường hợp GetFriendRequest thì key sẽ là ID của friend, value là friend mà request đến mình
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public List<AddFriendRequest> GetFriendRequest(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string customerId = m_mapConnectionIdCustomer[connectionId];
                //data DB
                Dictionary<string, AddFriendRequest> dbDict = m_adapter.GetFriendRequest(customerId);
                //add from db to cache
                foreach (AddFriendRequest request in dbDict.Values)
                {
                    if (!m_AddFriendRequestDict.ContainsKey(request.Id))
                        m_AddFriendRequestDict.TryAdd(request.Id, request);
                }
                return m_AddFriendRequestDict.Values.Where(t => t.Receiver.Equals(customerId)).ToList();
            }
            return null;
        }

        public AddFriendRequest GetFriendRequestById(string requestId)
        {
            if (m_AddFriendRequestDict.ContainsKey(requestId))
                return m_AddFriendRequestDict[requestId];
            else
            {
                AddFriendRequest request = m_adapter.GetFriendRequestById(requestId);
                if (request != null)
                    return request;
            }
            return null;
        }

        public ChatResult<AddFriendRequest> AddFriendRequest(string connectionId, string friendCustomerId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                if (!myCustomerId.Equals(friendCustomerId))
                {
                    CustomerInfo customer = GetCustomerByCustomerId(friendCustomerId);
                    if (customer == null)
                        return new ChatResult<AddFriendRequest>(false, "Id not found.", null);

                    if (!m_customerInfoDict[friendCustomerId].FriendList.Contains(myCustomerId))
                    {
                        //check request existance in DB and add to cache
                        Dictionary<string, AddFriendRequest> dbDict = m_adapter.GetFriendRequest(friendCustomerId);
                        foreach (AddFriendRequest request in dbDict.Values)
                            m_AddFriendRequestDict.TryAdd(request.Id, request);
                        //check request existance in cache
                        foreach (AddFriendRequest request in m_AddFriendRequestDict.Values)
                        {
                            if (request.Receiver.Equals(friendCustomerId) && request.Sender.Equals(myCustomerId))
                                return new ChatResult<AddFriendRequest>(false, "Request has already sent.", null);
                        }
                        //nếu dict chưa có key thì add mới
                        AddFriendRequest newrequest = new AddFriendRequest(Guid.NewGuid().ToString(), myCustomerId, friendCustomerId, DateTime.Now);
                        m_AddFriendRequestDict.TryAdd(newrequest.Id, newrequest);
                        m_adapter.AddFriendRequest(newrequest);//db
                        return new ChatResult<AddFriendRequest>(true, null, newrequest);
                    }
                    return new ChatResult<AddFriendRequest>(false, "This customer is already your friend.", null);
                }
                return new ChatResult<AddFriendRequest>(false, "This customerId is your Id.", null);
            }
            return new ChatResult<AddFriendRequest>(false, "Your Id doesn't exist.", null);
        }

        public ChatResult AddFriendRespond(string connectionId, string requestId, bool agree)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                if (m_AddFriendRequestDict.ContainsKey(requestId))
                {
                    AddFriendRequest request = m_AddFriendRequestDict[requestId];
                    CustomerInfo customer = GetCustomerByCustomerId(request.Receiver);
                    bool isFriend = customer.FriendList.Contains(request.Sender);
                    AddFriendRequest r;
                    //dù agree hay refuse thì dict sẽ remove friendrequest đi
                    m_AddFriendRequestDict.TryRemove(requestId, out r);
                    m_adapter.DeleteFriendRequest(requestId);//dtb
                    if (agree)
                    {
                        m_customerInfoDict[request.Sender].AddNewFriend(request.Receiver);
                        m_customerInfoDict[request.Receiver].AddNewFriend(request.Sender);
                        m_adapter.AddFriend(request.Sender, request.Receiver);//dtb
                    }
                    return new ChatResult(true, null);
                }//nếu requestDict ko còn nữa thì ko xử lí gì
                return new ChatResult(false, "No friend request.");
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }

        public ChatResult UnFriend(string connectionId, string friendCustomerId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_customerInfoDict.ContainsKey(friendCustomerId))
                {
                    m_customerInfoDict[myCustomerId].UnFriend(friendCustomerId);
                    m_customerInfoDict[friendCustomerId].UnFriend(myCustomerId);

                    m_adapter.UnFriend(myCustomerId, friendCustomerId);//dtb
                    return new ChatResult(true, null);
                }
                return new ChatResult(false, "FriendID doesn't exist.");
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }

        public ChatResult ChangeCustomerName(string connectionId, string newCustomerName)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                if (m_customerInfoDict[myCustomerId].ChangeCustomerName(newCustomerName))
                {
                    //m_adapter.ChangeCustomerName(myCustomerId, newCustomerName);//dtb
                    return new ChatResult(true, null);
                }
                return new ChatResult(false, "Name is too short.");
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }

        public ChatResult ChangeCustomerStatus(string connectionId, string stt)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                m_customerInfoDict[myCustomerId].ChangeOnlineStatus(stt);
                return new ChatResult(true, null);
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }

        public void ChangeLastOnline(string connectionId)
        {
            string customerId = m_mapConnectionIdCustomer[connectionId];
            DateTime lastlogin = DateTime.Now;
            m_customerInfoDict[customerId].LastOnline = lastlogin;
            m_adapter.ChangeLastOnline(customerId, lastlogin);//database                   
        }

        public CustomerInfo GetCustomerByConnectionId(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string customerId = m_mapConnectionIdCustomer[connectionId];
                return GetCustomerByCustomerId(customerId);
            }
            return null;
        }

        public CustomerInfo GetCustomerByCustomerId(string customerId)
        {
            if (m_customerInfoDict.ContainsKey(customerId))
                return m_customerInfoDict[customerId];
            else
            {
                CustomerInfo customer = m_adapter.GetCustomer(customerId);
                if (customer != null)
                {
                    customer.Status = ChatStatus.Offline;
                    m_customerInfoDict.TryAdd(customerId, customer);
                    return customer;
                }
            }
            return null;
        }

        public List<CustomerInfo> GetCustomerListByRole(string role)
        {
            return m_customerInfoDict.Values.Where(t => t.Role.Equals(CustomerRole.Admin)).ToList();
        }

        public List<CustomerInfo> GetFriendListByCustomerId(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                List<CustomerInfo> customerList = new List<CustomerInfo>();
                List<string> friendIdList = m_customerInfoDict[myCustomerId].FriendList;
                foreach (string friendId in friendIdList)
                {
                    if (!m_customerInfoDict.ContainsKey(friendId))//trường hợp manager chưa có customer thì vọc DB
                    {
                        CustomerInfo cus = GetCustomerByCustomerId(friendId);
                        if (cus != null)
                        {
                            CustomerInfo customer = new CustomerInfo(cus.CustomerId, cus.CustomerName, cus.Status);
                            customerList.Add(customer);
                        }
                    }
                    else
                    {
                        CustomerInfo cus = m_customerInfoDict[friendId];
                        CustomerInfo customer = new CustomerInfo(cus.CustomerId, cus.CustomerName, cus.Status);
                        customerList.Add(customer);
                    }
                }
                return customerList;
            }
            return null;
        }

        public List<CustomerInfo> SearchCustomerByCustomerName(string connectionId, string keyword)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                List<CustomerInfo> DBcusList = m_adapter.SearchCustomer(keyword);//get list from DB
                return DBcusList;
            }
            return null;
        }
        #endregion

        #region Group
        public GroupInfo AddGroup(string connectionId, string groupName, bool isPrivate)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                GroupInfo newGroup = new GroupInfo(groupName, isPrivate, null, new List<string>() { myCustomerId }, new List<string>() { myCustomerId });
                if (m_groupInfoDict.TryAdd(newGroup.GroupId, newGroup))
                {
                    GetCustomerByCustomerId(myCustomerId).JoinGroup(newGroup.GroupId);
                    m_adapter.AddGroup(newGroup);//add new group to db
                    m_adapter.JoinGroup(myCustomerId, newGroup.GroupId);//add customer to db
                    m_adapter.SetAdmin(myCustomerId, newGroup.GroupId, true);//add admin to db
                    return newGroup;
                }
            }
            return null;
        }

        public ChatResult DeleteGroup(string connectionId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                GroupInfo group;

                if (m_groupInfoDict[groupId].AdminList.Contains(myCustomerId))
                {
                    //customers in this group must leave group
                    foreach (string customer in m_groupInfoDict[groupId].CustomerList)
                        m_customerInfoDict[customer].LeaveGroup(groupId);
                    //remove group from dictionary
                    m_groupInfoDict.TryRemove(groupId, out group);
                    m_adapter.DeleteGroup(groupId);//save to dtb
                    return new ChatResult(true, null);
                }
                return new ChatResult(false, "You are not admin in this group.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult ChangeGroupName(string connectionId, string groupId, string newName)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                if (newName.Length >= 2)
                {
                    string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                    if (m_groupInfoDict.ContainsKey(groupId))
                    {
                        if (m_groupInfoDict[groupId].ChangeGroupName(myCustomerId, newName))
                        {
                            m_adapter.UpdateGroup(m_groupInfoDict[groupId]);//save to dtb
                            return new ChatResult(true, null);
                        }
                        return new ChatResult(false, "You are not authorized to change group name.");
                    }
                    return new ChatResult(false, "Group doen't exist.");
                }
                return new ChatResult(false, "Name is too short.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult ChangeGroupPrivate(string connectionId, string groupId, bool isPrivate)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].ChangeGroupPrivacy(myCustomerId, isPrivate))
                    {
                        m_adapter.UpdateGroup(m_groupInfoDict[groupId]);//save to dtb
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "You are not authorised to change group privacy.");
                }
                return new ChatResult(false, "Group doen't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult ChangeGroupDescription(string connectionId, string groupId, string description)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].ChangeGroupDescription(myCustomerId, description))
                    {
                        m_adapter.UpdateGroup(m_groupInfoDict[groupId]);//save to dtb
                        return new ChatResult(true, null);
                    }
                }
                return new ChatResult(false, "Group doen't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult AddAdminToGroup(string connectionId, string customerId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].AddAdmin(myCustomerId, customerId))
                    {
                        m_adapter.SetAdmin(customerId, groupId, true);//save to dtb
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't add admin to group.");
                }
                return new ChatResult(false, "Group doen't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult RemoveAdminFromGroup(string connectionId, string deletedAdminId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (GetGroupByGroupId(groupId) != null)
                {
                    if (m_groupInfoDict[groupId].RemoveAdmin(myCustomerId, deletedAdminId))
                    {
                        m_adapter.SetAdmin(deletedAdminId, groupId, false);//database
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't remove admin from group.");
                }
                return new ChatResult(false, "Group not found.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult<InviteGroupRequest> InviteToGroupRequest(string connectionId, string newCustomerId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                GroupInfo gr = GetGroupByGroupId(groupId);//check group exist or not
                if (gr != null)
                {
                    if (!m_groupInfoDict[groupId].HasCustomer(newCustomerId))//check customer in group not not
                    {
                        //check request in cache and add to cache
                        foreach (InviteGroupRequest request in m_InviteGroupRequestDict.Values)
                            m_InviteGroupRequestDict.TryAdd(request.Id, request);
                        //checkrequest in DB
                        Dictionary<string, InviteGroupRequest> dict = m_adapter.GetGroupRequest(newCustomerId);
                        foreach (InviteGroupRequest request in dict.Values)
                        {
                            if (request.Receiver.Equals(newCustomerId) && request.GroupId.Equals(groupId))
                                return new ChatResult<InviteGroupRequest>(false, "Request is already sent", null);
                        }

                        InviteGroupRequest newrequest = new InviteGroupRequest(Guid.NewGuid().ToString(), myCustomerId, newCustomerId, groupId, DateTime.Now);
                        newrequest.GroupName = gr.GroupName;
                        m_InviteGroupRequestDict.TryAdd(newrequest.Id, newrequest);
                        m_adapter.InviteToGroupRequest(newrequest);//DB
                        return new ChatResult<InviteGroupRequest>(true, null, newrequest);
                    }
                    return new ChatResult<InviteGroupRequest>(false, "Customer is already in group.", null);
                }
                return new ChatResult<InviteGroupRequest>(false, "Group not found.", null);
            }
            return new ChatResult<InviteGroupRequest>(false, "Your ID doens't exist.", null);
        }

        public ChatResult InviteToGroupRespond(string connectionId, string requestId, bool agree)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                if (m_InviteGroupRequestDict.ContainsKey(requestId))
                {
                    InviteGroupRequest request = m_InviteGroupRequestDict[requestId];
                    GroupInfo group = GetGroupByGroupId(request.GroupId);
                    if (group != null)
                    {
                        bool isInGroup = group.HasCustomer(request.Receiver);
                        if (!isInGroup)
                        {
                            if (agree)
                            {
                                m_customerInfoDict[request.Receiver].JoinGroup(request.GroupId);
                                group.JoinGroup(request.Receiver);
                                m_adapter.JoinGroup(request.Receiver, request.GroupId);//DB
                            }
                            InviteGroupRequest groupRequest;
                            m_InviteGroupRequestDict.TryRemove(requestId, out groupRequest);
                            m_adapter.DeleteGroupRequest(request.Id);//DB
                            return new ChatResult(true, null);
                        }
                        return new ChatResult(false, "Customer is already in group");
                    }
                    return new ChatResult(false, "Group not found.");
                }
                return new ChatResult(false, "Request not found");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public List<InviteGroupRequest> GetGroupRequest(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string customerId = m_mapConnectionIdCustomer[connectionId];
                //data DB
                Dictionary<string, InviteGroupRequest> dbDict = m_adapter.GetGroupRequest(customerId);
                //add from DB to cache
                foreach (InviteGroupRequest request in dbDict.Values)
                {
                    request.GroupName = GetGroupByGroupId(request.GroupId).GroupName;
                    if (!m_InviteGroupRequestDict.ContainsKey(request.Id))
                        m_InviteGroupRequestDict.TryAdd(request.Id, request);
                }
                return m_InviteGroupRequestDict.Values.Where(t => t.Receiver.Equals(customerId)).ToList();
            }
            return null;
        }

        public InviteGroupRequest GetGroupRequestById(string requestId)
        {
            if (m_InviteGroupRequestDict.ContainsKey(requestId))
                return m_InviteGroupRequestDict[requestId];
            else
            {
                InviteGroupRequest request = m_adapter.GetGroupRequestById(requestId);
                if (request != null)
                    return request;
            }
            return null;
        }

        public ChatResult RemoveCustomerFromGroup(string connectionId, string customerId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                CustomerInfo cus = GetCustomerByCustomerId(customerId);//check newcustomer exist or not
                if (cus != null)
                {
                    GroupInfo gr = GetGroupByGroupId(groupId);
                    if (gr != null)
                    {
                        if (m_groupInfoDict[groupId].RemoveCustomer(myCustomerId, customerId))
                        {
                            m_groupInfoDict[groupId].RemoveAdmin(myCustomerId, customerId);//remove from admin list (if being admin)
                            m_customerInfoDict[customerId].LeaveGroup(groupId);//remove group from customer
                            m_adapter.LeaveGroup(customerId, groupId);//remove from db
                            return new ChatResult(true, null);
                        }
                        return new ChatResult(false, "You are not authorized to remove customer.");
                    }
                    return new ChatResult(false, "Group not found.");
                }
                return new ChatResult(false, "Customer not found.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult JoinGroup(string connectionId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                GroupInfo group = GetGroupByGroupId(groupId);
                if (group != null)
                {
                    if (m_groupInfoDict[groupId].JoinGroup(myCustomerId))
                    {
                        m_customerInfoDict[myCustomerId].GroupList.Add(groupId);//add group to customer
                        m_adapter.JoinGroup(myCustomerId, groupId);//add DB
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't join group.");
                }
                return new ChatResult(false, "Group doesn't exist");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult LeaveGroup(string connectionId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string customerId = m_mapConnectionIdCustomer[connectionId];
                GroupInfo group = GetGroupByGroupId(groupId);
                if (group != null)
                {
                    m_groupInfoDict[groupId].LeaveGroup(customerId);//remove customer from group
                    m_groupInfoDict[groupId].AdminList.Remove(customerId);//if admin, remove from admin list
                    m_customerInfoDict[customerId].LeaveGroup(groupId);//remove group from customer
                    m_adapter.LeaveGroup(customerId, groupId);//remove from DB
                    return new ChatResult(true, null);
                }
                return new ChatResult(false, "Group doesn't exist");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public List<GroupInfo> SearchGroupByGroupName(string connectionId, string groupName)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                List<GroupInfo> DBgroupList = m_adapter.SearchGroup(groupName);//get data from DB               
                return DBgroupList.Where(g => g.GroupName.Contains(groupName)).ToList();
            }
            return new List<GroupInfo>();
        }

        public List<GroupInfo> GetGroupListOfCustomer(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                List<GroupInfo> groupList = new List<GroupInfo>();
                List<string> groupIds = m_customerInfoDict[myCustomerId].GroupList;
                foreach (string groupId in groupIds)
                {
                    GroupInfo group = GetGroupByGroupId(groupId);
                    if (group != null)
                        groupList.Add(group);
                }
                return groupList;
            }
            return null;
        }

        public GroupInfo GetGroupByGroupId(string groupId)
        {
            if (m_groupInfoDict.ContainsKey(groupId))
                return m_groupInfoDict[groupId];
            else
            {
                GroupInfo group = m_adapter.GetGroup(groupId);
                if (group != null)
                {
                    m_groupInfoDict.TryAdd(group.GroupId, group);
                    return group;
                }
            }
            return null;
        }
        #endregion

        #region Message
        public ChatResult AddMessage(MessageInfo msg)
        {
            lock (m_lock)
            {
                m_messageId += m_messageId + 1;
                msg.Id = m_messageId.ToString();
                m_adapter.AddMessage(msg);//database
                return new ChatResult(true, null);
            }
        }

        public List<MessageInfo> GetOfflineMessage(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string customerId = m_mapConnectionIdCustomer[connectionId];
                List<MessageInfo> offlineFriendMsg = m_adapter.GetOfflineMessageFriend(customerId);//get offline msg from friend
                List<MessageInfo> offlineGroupMsg = new List<MessageInfo>();
                foreach (string groupId in m_customerInfoDict[customerId].GroupList)
                    offlineGroupMsg.AddRange(m_adapter.GetOfflineMessageGroup(customerId, groupId));//get offline msg from group
                List<MessageInfo> offlineBroadcast = m_adapter.GetBroadcastMessage(customerId);//get broadcast msg

                List<MessageInfo> offlineMessageList = new List<MessageInfo>();//new offline msg list
                offlineMessageList.AddRange(offlineFriendMsg);
                offlineMessageList.AddRange(offlineGroupMsg);
                offlineMessageList.AddRange(offlineBroadcast);

                return offlineMessageList;

            }
            return null;
        }

        public List<MessageInfo> SearchMessage(string connectionId, string receiverId, DateTime fromDate, DateTime toDate, int messageType)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                if (messageType == 100)
                {
                    List<MessageInfo> messageList1 = m_adapter.SearchMessage(myCustomerId, receiverId, fromDate, toDate, messageType);
                    List<MessageInfo> messageList2 = m_adapter.SearchMessage(receiverId, myCustomerId, fromDate, toDate, messageType);
                    messageList1.AddRange(messageList2);
                    return messageList1.OrderBy(t => t.Datetime).ToList();
                }
                else
                {
                    List<MessageInfo> messageList = m_adapter.SearchMessage(myCustomerId, receiverId, fromDate, toDate, messageType);
                    if (messageList == null)
                        return null;
                    return messageList.OrderBy(t => t.Datetime).ToList();
                }
            }
            return null;
        }
        #endregion
    }
}
