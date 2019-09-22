using ATDChatClient.Manager.Model;
using ATDChatDefinition;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatClient.Manager
{
    public class DataManager : ChatEventFirer
    {
        //mycustomer
        private CustomerInfo m_customer = new CustomerInfo();

        //friendId, friend
        private ConcurrentDictionary<string, CustomerInfo> m_friendDict = new ConcurrentDictionary<string, CustomerInfo>();

        //groupId/groupInfo
        private ConcurrentDictionary<string, GroupInfo> m_groupDict = new ConcurrentDictionary<string, GroupInfo>();

        private static DataManager m_Instance;
        public static DataManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new DataManager();
                return m_Instance;
            }
        }

        public CustomerInfo Customer
        {
            get { return m_customer; }
            set { m_customer = value; }
        }
        public ConcurrentDictionary<string, CustomerInfo> FriendDict
        {
            get { return m_friendDict; }
            set { m_friendDict = value; }
        }
        public ConcurrentDictionary<string, GroupInfo> GroupDict
        {
            get { return m_groupDict; }
            set { m_groupDict = value; }
        }

        public IHubProxy HubProxy { get; set; }
        private HubConnection Connection { get; set; }

        #region Connection
        public void ConnectServer(string server, string port, string customerId, string accountId)
        {
            string ServerURI = server + ":" + port;
            //string ServerURI = "http://localhost:8080/";
            //if (Connection == null)
            Connection = new HubConnection(ServerURI);
            HubProxy = Connection.CreateHubProxy("ChatHub");
            try
            {
                Connection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        object[] obj = { false, "Can't connect to server." };
                        FireEvent("Login", obj);
                    }
                    else
                    {
                        AuthenRequest(customerId, accountId);
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }

        public void DisconnectServer()
        {
            if (Connection != null)
                Connection.Stop();
        }
        #endregion

        public void AuthenRequest(string customerId, string accountId)
        {
            HubProxy.Invoke<bool>(ServerEventNames.AuthenRequest, customerId, accountId, GetBytes(customerId)).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                }
                else
                {
                    bool isAuthen = task.Result;
                    if (isAuthen)
                    {
                        SubscribeEventFromServer();
                        GetCustomerInfo();
                    }
                    else
                    {
                        object[] obj = { false, "Username or password is incorrect." };
                        FireEvent("Login", obj);
                    }
                }
            });
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void GetCustomerInfo()
        {
            HubProxy.Invoke<CustomerInfo>(ServerEventNames.GetCustomerInfo).ContinueWith(task =>
            {
                if (task.IsFaulted) { }
                else
                {
                    Customer = task.Result;
                    object[] obj = { true, "Login successed." };
                    FireEvent("Login", obj);
                    //GetInventory();
                }
            });
        }

        public void GetInventory()
        {
            HubProxy.Invoke<ChatResult>(ServerEventNames.GetInventory).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                }
                else
                {
                    ChatResult result = task.Result;
                }
            });
        }

        private void SubscribeEventFromServer()
        {
            //HubProxy.On<CustomerInfo, List<CustomerInfo>, List<GroupInfo>, List<MessageInfo>, List<CustomerInfo>>
            //    (ClientEventNames.OnGetInventory, (customer, friendList, groupList, offlineMessageList, requestList) =>
            //        OnGetInventory(customer, friendList, groupList, offlineMessageList, requestList));

            //HubProxy.On<CustomerInfo>(ClientEventNames.OnGetCustomerInfo, (customer) => OnGetCustomerInfo(customer));
            HubProxy.On<List<CustomerInfo>>(ClientEventNames.OnGetFriendList, (friendList) => OnGetFriendList(friendList));
            HubProxy.On<List<GroupInfo>>(ClientEventNames.OnGetGroupList, (groupList) => OnGetGroupList(groupList));
            HubProxy.On<List<MessageInfo>>(ClientEventNames.OnGetOfflineMessage, (offlineMessageList) => OnGetOfflineMessage(offlineMessageList));

            HubProxy.On<List<AddFriendRequest>>(ClientEventNames.OnAddFriendRequest, (requestList) => OnAddFriendRequest(requestList));
            HubProxy.On<CustomerInfo, bool>(ClientEventNames.OnAddFriendRespond, (respondCustomer, agree) => OnAddFriendRespond(respondCustomer, agree));
            HubProxy.On<string>(ClientEventNames.OnUnFriend, (friendId) => OnUnFriend(friendId));
            HubProxy.On<string, string>(ClientEventNames.OnChangeCustomerStatus, (customerId, status) => OnChangeCustomerStatus(customerId, status));

            HubProxy.On<GroupInfo>(ClientEventNames.OnAddGroup, (group) => OnAddGroup(group));
            HubProxy.On<string>(ClientEventNames.OnDeleteGroup, (groupId) => OnDeleteGroup(groupId));
            HubProxy.On<string, string>(ClientEventNames.OnAddAdminToGroup, (adminId, groupId) => OnAddAdminToGroup(adminId, groupId));
            HubProxy.On<string, string>(ClientEventNames.OnRemoveAdminFromGroup, (adminId, groupId) => OnRemoveAdminFromGroup(adminId, groupId));
            HubProxy.On<List<InviteGroupRequest>>(ClientEventNames.OnInviteToGroupRequest, (requestList) => OnInviteToGroupRequest(requestList));
            HubProxy.On<string, GroupInfo, bool>(ClientEventNames.OnInviteToGroupRespond, (customerId, group, agree) => OnInviteToGroupRespond(customerId, group, agree));
            HubProxy.On<string, string>(ClientEventNames.OnRemoveCustomerFromGroup, (customerId, groupId) => OnRemoveCustomerFromGroup(customerId, groupId));
            HubProxy.On<string, GroupInfo>(ClientEventNames.OnJoinGroup, (customerId, groupId) => OnJoinGroup(customerId, groupId));
            HubProxy.On<string, string>(ClientEventNames.OnLeaveGroup, (groupId, customerId) => OnLeaveGroup(customerId, groupId));
            HubProxy.On<string, GroupInfo>(ClientEventNames.OnUpdateGroup, (updateType, group) => OnUpdateGroup(updateType, group));

            HubProxy.On<MessageInfo>(ClientEventNames.OnReceiveMessage, (msg) => OnReceiveMessage(msg));
        }

        #region Inventory
        //private void OnGetCustomerInfo(CustomerInfo customer)
        //{
        //    if (customer != null)
        //        Customer = customer;
        //    FireEvent(ClientEventNames.OnGetCustomerInfo, null);
        //}

        private void OnGetFriendList(List<CustomerInfo> friendList)
        {
            if (friendList != null)
            {
                m_friendDict.Clear();
                foreach (CustomerInfo cus in friendList)
                {
                    if (!m_friendDict.ContainsKey(cus.CustomerId))
                        m_friendDict.TryAdd(cus.CustomerId, cus);
                }
                FireEvent(ClientEventNames.OnGetFriendList, null);
            }
        }

        private void OnGetGroupList(List<GroupInfo> groupList)
        {
            if (groupList != null)
            {
                m_groupDict.Clear();
                foreach (GroupInfo group in groupList)
                {
                    if (!m_groupDict.ContainsKey(group.GroupId))
                        m_groupDict.TryAdd(group.GroupId, group);
                }
                FireEvent(ClientEventNames.OnGetGroupList, null);
            }
        }

        private void OnGetOfflineMessage(List<MessageInfo> offlineMessageList)
        {
            if (offlineMessageList != null)
                FireEvent(ClientEventNames.OnGetOfflineMessage, new object[] { offlineMessageList });
        }
        #endregion

        #region Customer
        public async Task<ChatResult> AddFriendRequest(string friendId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.AddFriendRequest, friendId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> AddFriendRespond(string requestId, bool agree)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.AddFriendRespond, requestId, agree);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> UnFriend(string friendId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.UnFriend, friendId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> ChangeCustomerStatus(string stt)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.ChangeCustomerStatus, stt);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> ChangeCustomerName(string newCustomerName)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.ChangeCustomerName, newCustomerName);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult<List<CustomerInfo>>> SearchCustomer(string customerNameKeyword)
        {
            try
            {
                ChatResult<List<CustomerInfo>> result = await HubProxy.Invoke<ChatResult<List<CustomerInfo>>>(ServerEventNames.SearchCustomer, customerNameKeyword);
                return result;
            }
            catch (Exception ex)
            {
                return ChatResult<List<CustomerInfo>>.CreateInstance(false, ex.Message, null);
            }

        }
        #endregion

        #region Group
        public async Task<ChatResult> AddGroup(string groupName, bool isPrivate)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.AddGroup, groupName, isPrivate);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> DeleteGroup(string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.DeleteGroup, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> ChangeGroupName(string groupId, string newName)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.ChangeGroupName, groupId, newName);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> ChangeGroupPrivate(string groupId, bool isPrivate)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.ChangeGroupPrivacy, groupId, isPrivate);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> ChangeGroupDescription(string groupId, string description)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.ChangeGroupDescription, groupId, description);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> AddAdminToGroup(string customerId, string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.AddAdminToGroup, customerId, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> RemoveAdminFromGroup(string customerID, string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.RemoveAdminFromGroup, customerID, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> InviteToGroupRequest(string customerId, string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.InviteToGroupRequest, customerId, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> InviteToGroupRespond(string requestId, bool agree)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.InviteToGroupRespond, requestId, agree);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> RemoveCustomerFromGroup(string customerId, string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.RemoveCustomerFromGroup, customerId, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> JoinGroup(string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.JoinGroup, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> LeaveGroup(string groupId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.LeaveGroup, groupId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult<List<GroupInfo>>> SearchGroup(string groupName)
        {
            try
            {
                ChatResult<List<GroupInfo>> result = await HubProxy.Invoke<ChatResult<List<GroupInfo>>>(ServerEventNames.SearchGroup, groupName);
                return result;
            }
            catch (Exception ex)
            {
                return ChatResult<List<GroupInfo>>.CreateInstance(false, ex.Message, null);
            }
        }
        #endregion

        #region Message
        public async Task<ChatResult> SendMessageToFriend(MessageInfo msg)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.SendMessageToFriend, msg);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> SendMessageToGroup(MessageInfo msg)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.SendMessageToGroup, msg);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> DeleteMessage(string messageId)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.DeleteMessage, messageId);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult> BroadcastMessage(MessageInfo msg)
        {
            try
            {
                ChatResult result = await HubProxy.Invoke<ChatResult>(ServerEventNames.BroadcastMessage, msg);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult(false, ex.Message);
            }
        }

        public async Task<ChatResult<List<MessageInfo>>> SearchMessage(string receiverId, DateTime fromDate, DateTime toDate, int messageType)
        {
            try
            {
                ChatResult<List<MessageInfo>> result = await HubProxy.Invoke<ChatResult<List<MessageInfo>>>(ServerEventNames.SearchMessage, receiverId, fromDate, toDate, messageType);
                return result;
            }
            catch (Exception ex)
            {
                return new ChatResult<List<MessageInfo>>(false, ex.ToString(), null);
            }
        }
        #endregion

        #region Subcribe Event From Server
        //private void OnGetInventory(CustomerInfo customer, List<CustomerInfo> friendList, List<GroupInfo> groupList, List<MessageInfo> offlineMessageList, List<CustomerInfo> requestList)
        //{
        //    Customer = customer;
        //    if (friendList != null)
        //    {
        //        foreach (CustomerInfo cus in friendList)
        //            m_friendDict.Add(cus.CustomerId, cus);
        //    }

        //    if (groupList != null)
        //    {
        //        foreach (GroupInfo group in groupList)
        //            m_groupDict.Add(group.GroupId, group);
        //    }

        //    if (offlineMessageList != null)
        //        m_offlineMessageDict.AddRange(offlineMessageList);

        //    if (requestList != null)
        //        m_friendRequest.AddRange(requestList);
        //}

        public void OnAddFriendRequest(List<AddFriendRequest> requestList)
        {
            FireEvent(ClientEventNames.OnAddFriendRequest, new object[] { requestList });
        }

        public void OnAddFriendRespond(CustomerInfo respondCustomer, bool agree)
        {
            if (agree)
                m_friendDict.TryAdd(respondCustomer.CustomerId, respondCustomer);
            FireEvent(ClientEventNames.OnAddFriendRespond, new object[] { respondCustomer, agree });
        }

        public void OnUnFriend(string friendId)
        {
            CustomerInfo cus;
            m_friendDict.TryRemove(friendId, out cus);
            FireEvent(ClientEventNames.OnUnFriend, new object[] { friendId });
        }

        public void OnChangeCustomerStatus(string customerId, string status)
        {
            if (customerId == Customer.CustomerId)
                Customer.Status = status;
            FireEvent(ClientEventNames.OnChangeCustomerStatus, new object[] { customerId, status });
        }

        private void OnAddGroup(GroupInfo group)
        {
            if (!m_groupDict.ContainsKey(group.GroupId))
                m_groupDict.TryAdd(group.GroupId, group);
            FireEvent(ClientEventNames.OnAddGroup, new object[] { group });
        }

        private void OnDeleteGroup(string groupId)
        {
            GroupInfo group;
            m_groupDict.TryRemove(groupId, out group);
            FireEvent(ClientEventNames.OnDeleteGroup, new object[] { groupId });
        }

        private void OnAddAdminToGroup(string adminId, string groupId)
        {
            if (m_groupDict.ContainsKey(groupId))
            {
                if (!m_groupDict[groupId].AdminList.Contains(adminId))
                    m_groupDict[groupId].AdminList.Add(adminId);
            }
            FireEvent(ClientEventNames.OnAddAdminToGroup, new object[] { adminId, groupId });
        }

        private void OnRemoveAdminFromGroup(string adminId, string groupId)
        {
            if (m_groupDict.ContainsKey(groupId))
                m_groupDict[groupId].AdminList.Remove(adminId);
            FireEvent(ClientEventNames.OnRemoveAdminFromGroup, new object[] { adminId, groupId });
        }

        public void OnInviteToGroupRequest(List<InviteGroupRequest> requestList)
        {
            FireEvent(ClientEventNames.OnInviteToGroupRequest, new object[] { requestList });
        }

        private void OnInviteToGroupRespond(string customerId, GroupInfo group, bool agree)
        {
            if (agree)
            {
                if (customerId.Equals(Customer.CustomerId))
                    m_groupDict.TryAdd(group.GroupId, group);
                else
                    m_groupDict[group.GroupId].CustomerList.Add(customerId);
            }
            FireEvent(ClientEventNames.OnInviteToGroupRespond, new object[] { customerId, group, agree });

        }

        private void OnRemoveCustomerFromGroup(string customerId, string groupId)
        {
            GroupInfo group;
            if (customerId.Equals(m_customer.CustomerId))
                m_groupDict.TryRemove(groupId, out group);
            else
            {
                if (m_groupDict.ContainsKey(groupId))
                {
                    m_groupDict[groupId].CustomerList.Remove(customerId);
                    m_groupDict[groupId].AdminList.Remove(customerId);
                }
            }
            FireEvent(ClientEventNames.OnRemoveCustomerFromGroup, new object[] { customerId, groupId });
        }

        private void OnJoinGroup(string customerId, GroupInfo group)
        {
            if (!m_groupDict.ContainsKey(group.GroupId))
                m_groupDict.TryAdd(group.GroupId, group);
            else
                m_groupDict[group.GroupId].CustomerList = group.CustomerList;
            FireEvent(ClientEventNames.OnJoinGroup, new object[] { customerId, group.GroupId });
        }

        private void OnLeaveGroup(string customerId, string groupId)
        {
            GroupInfo group;
            if (customerId.Equals(m_customer.CustomerId))
                m_groupDict.TryRemove(groupId, out group);
            else
            {
                if (m_groupDict.ContainsKey(groupId))
                    m_groupDict[groupId].CustomerList.Remove(customerId);
            }
            FireEvent(ClientEventNames.OnLeaveGroup, new object[] { customerId, groupId });
        }

        public void OnUpdateGroup(string updateType, GroupInfo group)
        {
            switch (updateType)
            {
                case ClientEventNames.OnChangeGroupName:
                case ClientEventNames.OnChangeGroupPrivacy:
                case ClientEventNames.OnChangeGroupDescription:
                    if (m_groupDict.ContainsKey(group.GroupId))
                        m_groupDict[group.GroupId] = group;
                    FireEvent(ClientEventNames.OnUpdateGroup, new object[] { group });
                    break;
            }
        }

        public void OnReceiveMessage(MessageInfo msg)
        {
            //MessageDict.Add(msg.Id, msg);
            FireEvent(ClientEventNames.OnReceiveMessage, new object[] { msg });
        }
        #endregion
    }
}
