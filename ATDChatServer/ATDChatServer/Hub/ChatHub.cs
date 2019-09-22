using ATDChatDefinition;
using ATDChatServer.ChatManager;
using Microsoft.AspNet.SignalR;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer
{
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {            
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            CustomerInfo customer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
            if (customer != null)
            {
                if (customer.ConnectionList.Count == 1)
                {
                    ChatResult result = ChangeCustomerStatus(ChatStatus.Offline);
                    Manager.Instance.ChangeLastOnline(Context.ConnectionId);
                }
                customer.RemoveConnectionID(Context.ConnectionId);//remove connectionid from user
                Manager.Instance.RemoveConnectionId(Context.ConnectionId);//remove connectionid from mapper
            }
            return base.OnDisconnected(stopCalled);
        }

        public bool AuthenRequest(string customerId, string sessionAccount, byte[] cusId)
        {
            bool authen = Program.ChatServer.OnAuthenRequest(customerId, sessionAccount, cusId);
            if (authen)
                Manager.Instance.AddConnectionId(customerId, Context.ConnectionId);

            return authen;
        }

        [HubAuthorize]
        public ChatResult GetInventory()
        {
            //Clients.Client(Context.ConnectionId).OnGetInventory(GetCustomerInfo(), GetFriendList(), GetGroupList(),
            //    GetOfflineMessage(), GetFriendRequest());
            //Clients.Client(Context.ConnectionId).OnGetCustomerInfo(GetCustomerInfo());
            Clients.Client(Context.ConnectionId).OnGetFriendList(GetFriendList());
            Clients.Client(Context.ConnectionId).OnGetGroupList(GetGroupList());
            Clients.Client(Context.ConnectionId).OnGetOfflineMessage(GetOfflineMessage());
            Clients.Client(Context.ConnectionId).OnAddFriendRequest(GetFriendRequest());
            Clients.Client(Context.ConnectionId).OnInviteToGroupRequest(GetInviteToGroupRequest());
            return new ChatResult(true, null);
        }

        #region Customer
        [HubAuthorize]
        public CustomerInfo GetCustomerInfo()
        {
            return Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
        }

        [HubAuthorize]
        public List<CustomerInfo> GetFriendList()
        {
            List<CustomerInfo> customerList = Manager.Instance.GetFriendListByCustomerId(Context.ConnectionId);
            return customerList;
        }

        [HubAuthorize]
        public List<AddFriendRequest> GetFriendRequest()
        {
            return Manager.Instance.GetFriendRequest(Context.ConnectionId);
        }

        [HubAuthorize]
        public ChatResult<AddFriendRequest> AddFriendRequest(string friendId)
        {
            ChatResult<AddFriendRequest> result = Manager.Instance.AddFriendRequest(Context.ConnectionId, friendId);
            if (result.Success)
            {
                AddFriendRequest request = result.Data;
                List<string> friendConnectionIds = Manager.Instance.GetCustomerByCustomerId(friendId).ConnectionList;
                Clients.Clients(friendConnectionIds).OnAddFriendRequest(new List<AddFriendRequest>() { request });
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult AddFriendRespond(string requestId, bool agree)
        {
            AddFriendRequest request = Manager.Instance.GetFriendRequestById(requestId);
            if (request == null)
                return new ChatResult(false, "Request not found");
            CustomerInfo myCustomer = Manager.Instance.GetCustomerByCustomerId(request.Receiver);
            CustomerInfo friendCustomer = Manager.Instance.GetCustomerByCustomerId(request.Sender);
            CustomerInfo returnMyCustomer = new CustomerInfo(myCustomer.CustomerId, myCustomer.CustomerName, myCustomer.Status);
            CustomerInfo returnFriendCustomer = new CustomerInfo(friendCustomer.CustomerId, friendCustomer.CustomerName, friendCustomer.Status);

            ChatResult result = Manager.Instance.AddFriendRespond(Context.ConnectionId, requestId, agree);
            if (result.Success)
            {
                //send to request customer
                Clients.Clients(friendCustomer.ConnectionList).OnAddFriendRespond(returnMyCustomer, agree);
                if (agree)
                {
                    //send to respond customer
                    Clients.Clients(myCustomer.ConnectionList).OnAddFriendRespond(returnFriendCustomer, agree);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult UnFriend(string friendId)
        {
            ChatResult result = Manager.Instance.UnFriend(Context.ConnectionId, friendId);
            if (result.Success)
            {
                CustomerInfo myCustomer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
                CustomerInfo friendCustomer = Manager.Instance.GetCustomerByCustomerId(friendId);

                //send to friendConnectionId
                Clients.Clients(friendCustomer.ConnectionList).OnUnFriend(myCustomer.CustomerId);

                //send to myConnectionId
                Clients.Clients(myCustomer.ConnectionList).OnUnFriend(friendId);
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult ChangeCustomerStatus(string stt)
        {
            CustomerInfo myCustomer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
            ChatResult result = Manager.Instance.ChangeCustomerStatus(Context.ConnectionId, stt);
            if (result.Success)
            {
                //send to friendConnectionId
                foreach (string friendCustomerId in myCustomer.FriendList)
                {
                    List<string> connectionIds = Manager.Instance.GetCustomerByCustomerId(friendCustomerId).ConnectionList;
                    Clients.Clients(connectionIds).OnChangeCustomerStatus(myCustomer.CustomerId, stt);
                }
                //send to myConnectionId
                List<string> myConnectionList = myCustomer.ConnectionList;
                Clients.Clients(myConnectionList).OnChangeCustomerStatus(myCustomer.CustomerId, stt);
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult ChangeCustomerName(string newCustomerName)
        {
            ChatResult result = Manager.Instance.ChangeCustomerName(Context.ConnectionId, newCustomerName);
            if (result.Success)
            {
                CustomerInfo myCustomer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
                //send to friendConnectionId
                foreach (string friendCustomerId in myCustomer.FriendList)
                {
                    List<string> connectionids = Manager.Instance.GetCustomerByCustomerId(friendCustomerId).ConnectionList;
                    Clients.Clients(connectionids).OnChangeCustomerName(myCustomer.CustomerId, newCustomerName);
                }
                //send to myConnectionId
                Clients.Clients(myCustomer.ConnectionList).OnChangeCustomerName(newCustomerName);
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult<List<CustomerInfo>> SearchCustomer(string customerNameKeyword)
        {
            List<CustomerInfo> customerList = Manager.Instance.SearchCustomerByCustomerName(Context.ConnectionId, customerNameKeyword);
            if (customerList == null)
                return ChatResult<List<CustomerInfo>>.CreateInstance(false, "Customer not found", null);
            return ChatResult<List<CustomerInfo>>.CreateInstance(true, null, customerList);
        }
        #endregion

        #region Group
        [HubAuthorize]
        public List<GroupInfo> GetGroupList()
        {
            return Manager.Instance.GetGroupListOfCustomer(Context.ConnectionId);
        }

        [HubAuthorize]
        public ChatResult AddGroup(string groupName, bool isPrivate)
        {
            GroupInfo group = Manager.Instance.AddGroup(Context.ConnectionId, groupName, isPrivate);
            if (group != null)
            {
                CustomerInfo mycustomerInfo = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
                Clients.Clients(mycustomerInfo.ConnectionList).OnAddGroup(group);
                return new ChatResult(true, null);
            }
            return new ChatResult(false, "Fail to create group.");
        }

        [HubAuthorize]
        public ChatResult DeleteGroup(string groupId)
        {
            List<string> customerList = new List<string>();
            if (GetGroupByGroupId(groupId) != null)
                customerList.AddRange(GetGroupByGroupId(groupId).CustomerList);
            ChatResult result = Manager.Instance.DeleteGroup(Context.ConnectionId, groupId);
            if (result.Success)
            {
                foreach (string customerId in customerList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(customerId);
                    foreach (string connectionId in cus.ConnectionList)
                        Clients.Client(connectionId).OnDeleteGroup(groupId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult ChangeGroupName(string groupId, string newName)
        {
            ChatResult result = Manager.Instance.ChangeGroupName(Context.ConnectionId, groupId, newName);
            if (result.Success)
            {
                GroupInfo group = GetGroupByGroupId(groupId);
                foreach (string customerId in group.CustomerList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(customerId);
                    foreach (string connectionId in cus.ConnectionList)
                        Clients.Client(connectionId).OnUpdateGroup(ClientEventNames.OnChangeGroupName, GetGroupByGroupId(groupId));
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult ChangeGroupPrivacy(string groupId, bool isPrivate)
        {
            ChatResult result = Manager.Instance.ChangeGroupPrivate(Context.ConnectionId, groupId, isPrivate);
            //if (result.Success)
            //{
            //    GroupInfo group = GetGroupByGroupId(groupId);
            //    List<string> connectionIdList = new List<string>();
            //    foreach (string customerId in group.CustomerList)
            //    {
            //        CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(customerId);
            //        foreach (string connectionId in cus.ConnectionList)
            //            connectionIdList.Add(connectionId);
            //    }
            //    Clients.Clients(connectionIdList).OnUpdateGroup(ClientEventNames.OnChangeGroupPrivacy, GetGroupByGroupId(groupId));
            //}
            return result;
        }

        [HubAuthorize]
        public ChatResult ChangeGroupDescription(string groupId, string description)
        {
            ChatResult result = Manager.Instance.ChangeGroupDescription(Context.ConnectionId, groupId, description);
            if (result.Success)
            {
                GroupInfo group = GetGroupByGroupId(groupId);
                foreach (string customerId in group.CustomerList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(customerId);
                    foreach (string connectionId in cus.ConnectionList)
                        Clients.Client(connectionId).OnUpdateGroup(ClientEventNames.OnChangeGroupDescription, GetGroupByGroupId(groupId));
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult AddAdminToGroup(string customerId, string groupId)
        {
            ChatResult result = Manager.Instance.AddAdminToGroup(Context.ConnectionId, customerId, groupId);
            if (result.Success)
            {
                List<string> memberIdList = GetGroupByGroupId(groupId).AdminList;
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnAddAdminToGroup(customerId, groupId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult RemoveAdminFromGroup(string adminId, string groupId)
        {
            List<string> memberIdList = new List<string>();
            if (GetGroupByGroupId(groupId) != null)
                memberIdList.AddRange(GetGroupByGroupId(groupId).AdminList);
            ChatResult result = Manager.Instance.RemoveAdminFromGroup(Context.ConnectionId, adminId, groupId);
            if (result.Success)
            {
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnRemoveAdminFromGroup(adminId, groupId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult<InviteGroupRequest> InviteToGroupRequest(string customerId, string groupId)
        {
            ChatResult<InviteGroupRequest> result = Manager.Instance.InviteToGroupRequest(Context.ConnectionId, customerId, groupId);

            if (result.Success)
            {
                InviteGroupRequest request = (InviteGroupRequest)result.Data;
                CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(customerId);
                Clients.Clients(cus.ConnectionList).OnInviteToGroupRequest(new List<InviteGroupRequest>() { request });
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult InviteToGroupRespond(string requestId, bool agree)
        {
            InviteGroupRequest request = Manager.Instance.GetGroupRequestById(requestId);
            if (request == null)
                return new ChatResult(false, "Request not found.");
            string groupId = request.GroupId;
            string senderId = request.Sender;
            string receiverId = request.Receiver;

            ChatResult result = Manager.Instance.InviteToGroupRespond(Context.ConnectionId, requestId, agree);
            if (result.Success)
            {
                GroupInfo group = GetGroupByGroupId(groupId);
                if (group != null)
                {
                    if (agree)
                    {
                        List<string> memberIdList = group.CustomerList;
                        foreach (string memberId in memberIdList)
                        {
                            CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                            Clients.Clients(cus.ConnectionList).OnInviteToGroupRespond(receiverId, group, agree);
                        }
                    }
                    else
                    {
                        CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(senderId);
                        Clients.Clients(cus.ConnectionList).OnInviteToGroupRespond(receiverId, group, agree);
                    }
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult RemoveCustomerFromGroup(string customerId, string groupId)
        {
            GroupInfo group = GetGroupByGroupId(groupId);
            List<string> memberIdList = new List<string>();
            if (group != null)
                memberIdList = new List<string>(group.CustomerList);

            ChatResult result = Manager.Instance.RemoveCustomerFromGroup(Context.ConnectionId, customerId, groupId);
            if (result.Success)
            {
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnRemoveCustomerFromGroup(customerId, groupId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult JoinGroup(string groupId)
        {
            ChatResult result = Manager.Instance.JoinGroup(Context.ConnectionId, groupId);
            string customerId = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId).CustomerId;
            if (result.Success)
            {
                List<string> memberIdList = GetGroupByGroupId(groupId).CustomerList;
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnAddCustomerToGroup(customerId, GetGroupByGroupId(groupId));
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult LeaveGroup(string groupId)
        {
            string customerId = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId).CustomerId;

            List<string> memberIdList = GetGroupByGroupId(groupId).CustomerList;
            foreach (string memberId in memberIdList)
            {
                CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                Clients.Clients(cus.ConnectionList).OnRemoveCustomerFromGroup(customerId, groupId);
            }
            ChatResult result = Manager.Instance.LeaveGroup(Context.ConnectionId, groupId);
            return result;
        }

        [HubAuthorize]
        public ChatResult<List<GroupInfo>> SearchGroup(string groupName)
        {
            List<GroupInfo> groupList = Manager.Instance.SearchGroupByGroupName(Context.ConnectionId, groupName);
            if (groupList == null)
                return ChatResult<List<GroupInfo>>.CreateInstance(false, "Group not found", null);
            return ChatResult<List<GroupInfo>>.CreateInstance(true, null, groupList);
        }

        [HubAuthorize]
        public GroupInfo GetGroupByGroupId(string groupId)
        {
            return Manager.Instance.GetGroupByGroupId(groupId);
        }

        [HubAuthorize]
        public List<InviteGroupRequest> GetInviteToGroupRequest()
        {
            return Manager.Instance.GetGroupRequest(Context.ConnectionId);
        }
        #endregion

        #region Message
        [HubAuthorize]
        public List<MessageInfo> GetOfflineMessage()
        {
            List<MessageInfo> offlineMessage = Manager.Instance.GetOfflineMessage(Context.ConnectionId);
            if (offlineMessage != null)
                return offlineMessage;
            return null;
        }

        [HubAuthorize]
        public ChatResult SendMessageToFriend(MessageInfo msg)
        {
            CustomerInfo myCustomer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
            CustomerInfo friendCustomer = Manager.Instance.GetCustomerByCustomerId(msg.ReceiverId);
            //send message to friend
            List<string> friendConnectionIds = friendCustomer.ConnectionList;
            Clients.Clients(friendConnectionIds).OnReceiveMessage(msg);
            //send message to myCustomer(#connectionId)
            List<string> myConnectionIds = new List<string>(myCustomer.ConnectionList);
            myConnectionIds.Remove(Context.ConnectionId);
            Clients.Clients(myConnectionIds).OnReceiveMessage(msg);

            ChatResult result = Manager.Instance.AddMessage(msg);
            return result;
        }

        [HubAuthorize]
        public ChatResult SendMessageToGroup(MessageInfo msg)
        {
            GroupInfo group = GetGroupByGroupId(msg.ReceiverId);
            string senderId = msg.SenderId;
            if (group.CustomerList.Contains(senderId))
            {
                List<string> customerConnectionIds = new List<string>();
                foreach (string customerId in group.CustomerList)
                    customerConnectionIds.AddRange(Manager.Instance.GetCustomerByCustomerId(customerId).ConnectionList);
                customerConnectionIds.Remove(Context.ConnectionId);
                Clients.Clients(customerConnectionIds).OnReceiveMessage(msg);

                ChatResult result = Manager.Instance.AddMessage(msg);
                return result;
            }
            return null;
        }

        [HubAuthorize]
        public ChatResult<List<MessageInfo>> SearchMessage(string receiverId, DateTime fromdate, DateTime toDate, int messageType)
        {
            List<MessageInfo> messageList = Manager.Instance.SearchMessage(Context.ConnectionId, receiverId, fromdate, toDate, messageType);
            if (messageList == null)
                return ChatResult<List<MessageInfo>>.CreateInstance(false, "Message not found", null);
            return new ChatResult<List<MessageInfo>>(true, null, messageList);
        }

        [HubAuthorize]
        public ChatResult BroadcastMessage(MessageInfo message)
        {
            CustomerInfo myCustomer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
            if (myCustomer.Role.Equals(CustomerRole.Admin))
            {
                List<CustomerInfo> customerList = Manager.Instance.GetCustomerListByRole(message.ReceiverId);
                customerList.Remove(myCustomer);
                foreach (CustomerInfo customer in customerList)
                    Clients.Clients(customer.ConnectionList).OnReceiveMessage(message);
                Manager.Instance.AddMessage(message);

                return new ChatResult(true, null);
            } return null;
        }
        #endregion
    }
}
