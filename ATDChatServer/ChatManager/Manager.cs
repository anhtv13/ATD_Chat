using ATDChatDefinition;
using ATDChatServer.Adapter;
using ATDChatServer.ChatManager;
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

        //friendId, List<requestIds>
        private Dictionary<string, List<string>> m_requestAddFriendDict = new Dictionary<string, List<string>>();

        //customerid, messageId, Message
        private Dictionary<string, Dictionary<string, MessageInfo>> m_messageDict = new Dictionary<string, Dictionary<string, MessageInfo>>();

        //senderid, messageId, Message
        private Dictionary<string, Dictionary<string, MessageInfo>> m_offlineMessage = new Dictionary<string, Dictionary<string, MessageInfo>>();

        //database
        private PersistanceAdapter m_adapter = new PersistanceAdapter();

        private Manager()
        {
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
            m_mapConnectionIdCustomer.Add(connectionId, customerId);

            if (!m_customerInfoDict.ContainsKey(customerId))
                m_customerInfoDict.TryAdd(customerId, new CustomerInfo(customerId));
            m_customerInfoDict[customerId].AddNewConnectionID(connectionId);
        }

        public void RemoveConnectionId(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                m_customerInfoDict[m_mapConnectionIdCustomer[connectionId]].RemoveConnectionID(connectionId);
                m_mapConnectionIdCustomer.Remove(connectionId);
            }
        }
        #endregion

        #region Customer
        /// <summary>
        /// trong trường hợp GetFriendRequest thì key sẽ là ID của friend, value là friend mà request đến mình
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public List<CustomerInfo> GetFriendRequest(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                List<CustomerInfo> requestList = new List<CustomerInfo>();
                if (m_requestAddFriendDict.ContainsKey(myCustomerId))
                {
                    foreach (string friendId in m_requestAddFriendDict[myCustomerId])
                    {
                        CustomerInfo cus = m_customerInfoDict[friendId];
                        CustomerInfo friendCus = new CustomerInfo(cus.CustomerId, cus.CustomerName, cus.Status);
                        requestList.Add(friendCus);
                    }
                }
                return requestList;
            }
            return null;
        }

        public ChatResult AddFriendRequest(string connectionId, string friendCustomerId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                if (m_customerInfoDict.ContainsKey(friendCustomerId))
                {
                    string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                    if (!myCustomerId.Equals(friendCustomerId))
                    {
                        if (!m_customerInfoDict[friendCustomerId].FriendList.Contains(myCustomerId))
                        {
                            //nếu dict chưa có key thì add mới
                            if (!m_requestAddFriendDict.ContainsKey(friendCustomerId))
                                m_requestAddFriendDict.Add(friendCustomerId, new List<string>());

                            if (!m_requestAddFriendDict[friendCustomerId].Contains(myCustomerId))
                            {
                                m_requestAddFriendDict[friendCustomerId].Add(myCustomerId);
                                m_adapter.AddFriendRequest(friendCustomerId, myCustomerId);//dtb
                                return new ChatResult(true, null);
                            }
                            return new ChatResult(false, "Request has already sent.");
                        }
                        return new ChatResult(false, "This customer is already your friend.");
                    }
                    return new ChatResult(false, "This customerId is your Id.");
                }
                return new ChatResult(false, "This customerId doesn't exist.");
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }

        public ChatResult AddFriendRespond(string connectionId, string friendCustomerId, bool agree)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                if (m_customerInfoDict.ContainsKey(friendCustomerId))
                {
                    string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                    if (m_requestAddFriendDict.ContainsKey(myCustomerId))
                    {
                        if (m_requestAddFriendDict[myCustomerId].Contains(friendCustomerId))
                        {
                            bool isFriend = m_customerInfoDict[myCustomerId].FriendList.Contains(friendCustomerId);
                            //dù agree hay refuse thì dict sẽ remove friendrequest đi
                            m_requestAddFriendDict[myCustomerId].Remove(friendCustomerId);
                            m_adapter.DeleteFriendRequest(friendCustomerId, myCustomerId);//dtb
                            if (!isFriend)
                            {
                                if (agree)
                                {
                                    m_customerInfoDict[myCustomerId].AddNewFriend(friendCustomerId);
                                    m_customerInfoDict[friendCustomerId].AddNewFriend(myCustomerId);
                                    m_adapter.AddFriend(myCustomerId, friendCustomerId);//dtb
                                }
                                return new ChatResult(true, null);
                            }
                            return new ChatResult(false, "FriendID is already your friend.");
                        }
                    }
                }
                return new ChatResult(false, "FriendID doesn't exist.");
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
                    m_adapter.ChangeCustomerName(myCustomerId, newCustomerName);//dtb
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
                if (stt == ChatDefinition.Offline)
                {
                    m_customerInfoDict[myCustomerId].RemoveConnectionID(connectionId);
                    m_mapConnectionIdCustomer.Remove(connectionId);
                }
                m_adapter.ChangeCustomerStatus(myCustomerId, stt);//dtb
                return new ChatResult(true, null);
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }

        public CustomerInfo GetCustomerByConnectionId(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                return m_customerInfoDict[myCustomerId];
            }
            return null;
        }

        public CustomerInfo GetCustomerByCustomerId(string customerId)
        {
            if (m_customerInfoDict.ContainsKey(customerId))
                return m_customerInfoDict[customerId];
            return null;
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
                    CustomerInfo cus = m_customerInfoDict[friendId];
                    CustomerInfo newCus = new CustomerInfo(cus.CustomerId, cus.CustomerName, cus.Status);
                    customerList.Add(newCus);
                }
                return customerList;
            }
            return null;
        }

        public List<CustomerInfo> SearchCustomerByCustomerName(string connectionId, string customerId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                List<CustomerInfo> cusList = m_customerInfoDict.Values.Where(t => t.CustomerId.Contains(customerId)).ToList();
                List<CustomerInfo> sendCustomerList = new List<CustomerInfo>();
                foreach (CustomerInfo cus in cusList)
                {
                    CustomerInfo newCus = new CustomerInfo(cus.CustomerId, cus.CustomerName, cus.Status);
                    sendCustomerList.Add(newCus);
                }
                return sendCustomerList;
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
                GroupInfo newGroup = new GroupInfo(myCustomerId, groupName, isPrivate);
                if (m_groupInfoDict.TryAdd(newGroup.GroupId, newGroup))
                {
                    GetCustomerByConnectionId(connectionId).JoinGroup(newGroup.GroupId);
                    m_adapter.AddGroup(newGroup.GroupId, groupName, isPrivate);//save to dtb
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
                            m_adapter.ChangeGroupName(groupId, newName);//save to dtb
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
                        m_adapter.ChangeGroupPrivate(groupId, isPrivate);//save to dtb
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
                        m_adapter.ChangeGroupDescription(groupId, description);//save to dtb
                        return new ChatResult(true, null);
                    }
                }
                return new ChatResult(false, "Group doen't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult AddAdminToGroup(string connectionId, string groupId, string newAdminId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].AddAdmin(myCustomerId, newAdminId))
                    {
                        m_adapter.AddAdminToGroup(groupId, newAdminId);//save to dtb
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't add admin to group.");
                }
                return new ChatResult(false, "Group doen't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult RemoveAdminFromGroup(string connectionId, string groupId, string deletedAdminId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].RemoveAdmin(myCustomerId, deletedAdminId))
                    {
                        m_adapter.RemoveAdminFromGroup(groupId, deletedAdminId);//database
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't remove admin from group.");
                }
                return new ChatResult(false, "Group doen't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult AddCustomerToGroup(string connectionId, string groupId, string newCustomerId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_customerInfoDict.ContainsKey(newCustomerId) && m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].AddCustomer(myCustomerId, newCustomerId))
                    {
                        m_customerInfoDict[newCustomerId].JoinGroup(groupId);
                        m_adapter.JoinGroup(groupId, newCustomerId);
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't add customer to group.");
                }
                return new ChatResult(false, "Group and/or Customer doesn't exist");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult RemoveCustomerFromGroup(string connectionId, string groupId, string deletedCustomerId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_customerInfoDict.ContainsKey(deletedCustomerId) && m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].RemoveCustomer(myCustomerId, deletedCustomerId))
                    {
                        m_groupInfoDict[groupId].RemoveAdmin(myCustomerId, deletedCustomerId);//remove from admin list (if being admin)
                        m_customerInfoDict[deletedCustomerId].LeaveGroup(groupId);//remove group from customer
                        m_adapter.LeaveGroup(groupId, deletedCustomerId);//remove from db
                        return new ChatResult(true, null);
                    }
                    return new ChatResult(false, "Can't remove customer from group.");
                }
                return new ChatResult(false, "Group and/or Customer doesn't exist");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult JoinGroup(string connectionId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];

                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    if (m_groupInfoDict[groupId].JoinGroup(myCustomerId))
                    {
                        m_adapter.JoinGroup(groupId, myCustomerId);//add customer to group
                        m_customerInfoDict[myCustomerId].GroupList.Add(groupId);//add group to customer
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
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                if (m_groupInfoDict.ContainsKey(groupId))
                {
                    m_groupInfoDict[groupId].LeaveGroup(myCustomerId);//remove customer from group
                    m_groupInfoDict[groupId].AdminList.Remove(myCustomerId);//if admin, remove from admin list
                    m_customerInfoDict[myCustomerId].LeaveGroup(groupId);//remove group from customer
                    m_adapter.LeaveGroup(groupId, myCustomerId);//remove from DB
                    return new ChatResult(true, null);
                }
                return new ChatResult(false, "Group doesn't exist");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public List<GroupInfo> SearchGroupByGroupName(string connectionId, string groupName)
        {
            List<GroupInfo> returnGroupList = new List<GroupInfo>();
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                List<GroupInfo> groupList = m_groupInfoDict.Values.ToList<GroupInfo>();

                foreach (GroupInfo group in groupList)
                {
                    if (!group.IsPrivate)
                        returnGroupList.Add(group);
                    else
                    {
                        if (group.CustomerList.Contains(myCustomerId))
                            returnGroupList.Add(group);
                    }
                }
            }
            return returnGroupList.Where(t => t.GroupName.Contains(groupName)).ToList();
        }

        public List<GroupInfo> GetGroupListOfCustomer(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                List<string> groupIds = m_customerInfoDict[myCustomerId].GroupList;
                List<GroupInfo> groupList = new List<GroupInfo>();
                foreach (string groupId in groupIds)
                {
                    GroupInfo group = m_groupInfoDict[groupId];
                    groupList.Add(group);
                }
                return groupList;
            }
            return null;
        }

        public GroupInfo GetGroupByGroupId(string connectionId, string groupId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                if (m_groupInfoDict.ContainsKey(groupId))
                    return m_groupInfoDict[groupId];
            }
            return null;
        }
        #endregion

        #region Message
        public ChatResult AddMessage(string connectionId, MessageInfo msg)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                string receiverId = msg.ReceiverId;

                //nếu receiver ở đây là customer(ko phải group) thì kiểm tra customer on hay off
                if (m_customerInfoDict.ContainsKey(receiverId))
                {
                    //nếu receiver offline thì lưu vào OffLineMessageDict
                    string status = m_customerInfoDict[receiverId].Status;
                    if (status.Equals(ChatDefinition.Offline))
                    {
                        if (!m_offlineMessage.ContainsKey(receiverId))
                            m_offlineMessage.Add(receiverId, new Dictionary<string, MessageInfo>());
                        if (!m_offlineMessage[receiverId].ContainsKey(msg.Id))
                            m_offlineMessage[receiverId].Add(msg.Id, msg);
                        m_adapter.AddOfflineMessage(myCustomerId, msg.Id);//database
                    }
                }
                //nếu receiver ở đây là Group thì kiểm tra group tồn tại ko
                else
                {
                    if (!m_groupInfoDict.ContainsKey(receiverId) && !receiverId.Equals("All"))
                        return new ChatResult(false, "ReceiverId doesn't exist.");
                }

                if (!m_messageDict.ContainsKey(myCustomerId))
                    m_messageDict.Add(myCustomerId, new Dictionary<string, MessageInfo>());
                if (!m_messageDict[myCustomerId].ContainsKey(msg.Id))
                    m_messageDict[myCustomerId].Add(msg.Id, msg);
                m_adapter.AddMessage(myCustomerId, msg.Id);//database
                return new ChatResult(true, null);
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public ChatResult DeleteMessage(string connectionId, string messageId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                if (m_messageDict[myCustomerId].ContainsKey(messageId))
                {
                    m_messageDict[myCustomerId].Remove(messageId);
                    m_adapter.DeleteMessage(myCustomerId, messageId);//database
                    return new ChatResult(true, null);
                }
                return new ChatResult(false, "MessageId doesn't exist.");
            }
            return new ChatResult(false, "Your ID doens't exist.");
        }

        public List<MessageInfo> GetOfflineMessage(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                if (m_offlineMessage.ContainsKey(myCustomerId))
                    return m_offlineMessage[myCustomerId].Values.ToList();
            }
            return null;
        }

        public void DeleteOfflineMessage(string connectionId)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                m_offlineMessage.Remove(myCustomerId);
                m_adapter.DeleteOfflineMessage(myCustomerId);
            }
        }

        public List<MessageInfo> SearchMessage(string connectionId, string receiverId, DateTime fromDate, DateTime toDate)
        {
            if (m_mapConnectionIdCustomer.ContainsKey(connectionId))
            {
                string myCustomerId = m_mapConnectionIdCustomer[connectionId];
                List<MessageInfo> msgList = m_messageDict[myCustomerId].Values.ToList();
                return msgList.Where(t => t.ReceiverId.Equals(receiverId) &&
                    fromDate <= t.Datetime && t.Datetime <= toDate).ToList();
            }
            return null;
        }
        #endregion
    }
}
