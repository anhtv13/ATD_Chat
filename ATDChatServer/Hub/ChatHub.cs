using ATDChatDefinition;
using ATDChatServer.ChatManager;
using Microsoft.AspNet.SignalR;
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

        public override Task OnDisconnected(bool stopCalled)
        {
            CustomerInfo customer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
            if (customer != null)
            {
                ChangeCustomerStatus(ChatDefinition.Offline);

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
            Clients.Client(Context.ConnectionId).OnGetInventory(GetCustomerInfo(), GetFriendList(), GetGroupList(),
                GetOfflineMessage(), GetFriendRequest());
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
            return Manager.Instance.GetFriendListByCustomerId(Context.ConnectionId);
        }

        [HubAuthorize]
        public List<CustomerInfo> GetFriendRequest()
        {
            return Manager.Instance.GetFriendRequest(Context.ConnectionId);
        }

        [HubAuthorize]
        public ChatResult AddFriendRequest(string friendId)
        {
            ChatResult result = Manager.Instance.AddFriendRequest(Context.ConnectionId, friendId);
            if (result.Success)
            {
                CustomerInfo customer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
                CustomerInfo returnCustomer = new CustomerInfo(customer.CustomerId, customer.CustomerName, customer.Status);
                List<string> friendConnectionIds = Manager.Instance.GetCustomerByCustomerId(friendId).ConnectionList;
                Clients.Clients(friendConnectionIds).OnAddFriendRequest(returnCustomer);
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult AddFriendRespond(string friendId, bool agree)
        {
            ChatResult result = Manager.Instance.AddFriendRespond(Context.ConnectionId, friendId, agree);
            if (result.Success)
            {
                CustomerInfo myCustomer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
                CustomerInfo friendCustomer = Manager.Instance.GetCustomerByCustomerId(friendId);

                CustomerInfo returnMyCustomer = new CustomerInfo(myCustomer.CustomerId, myCustomer.CustomerName, myCustomer.Status);
                CustomerInfo returnFriendCustomer = new CustomerInfo(friendCustomer.CustomerId, friendCustomer.CustomerName, friendCustomer.Status);
                //send to request customer
                //gửi về 2 thông tin: bool agree, Customer
                Clients.Clients(friendCustomer.ConnectionList).OnAddFriendRespond(agree, returnMyCustomer);

                //send to respond customer
                //gửi về 2 thông tin nếu bool agree = true : customerid, string name
                if (agree == true)
                    Clients.Clients(myCustomer.ConnectionList).OnAddFriendRespond(agree, returnFriendCustomer);
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
                ////send to myConnectionId
                //List<string> myConnectionList = myCustomer.ConnectionList;
                //Clients.Clients(myConnectionList).OnChangeCustomerStatus(myCustomer.CustomerId, stt);
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
            System.Threading.Thread.Sleep(3000);
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
                Clients.Client(Context.ConnectionId).OnAddGroup(group);
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
                        Clients.All.OnUpdateGroup(ClientEventNames.OnChangeGroupName, GetGroupByGroupId(groupId));
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult ChangeGroupPrivacy(string groupId, bool isPrivate)
        {
            ChatResult result = Manager.Instance.ChangeGroupPrivate(Context.ConnectionId, groupId, isPrivate);
            if (result.Success)
            {
                GroupInfo group = GetGroupByGroupId(groupId);
                foreach (string customerId in group.CustomerList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(customerId);
                    foreach (string connectionId in cus.ConnectionList)
                        Clients.All.OnUpdateGroup(ClientEventNames.OnChangeGroupPrivacy, GetGroupByGroupId(groupId));
                }
            }

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
                        Clients.All.OnUpdateGroup(ClientEventNames.OnChangeGroupDescription, GetGroupByGroupId(groupId));
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult AddAdminToGroup(string groupId, string newAdminId)
        {
            ChatResult result = Manager.Instance.AddAdminToGroup(Context.ConnectionId, groupId, newAdminId);
            if (result.Success)
            {
                List<string> memberIdList = GetGroupByGroupId(groupId).CustomerList;
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnAddAdminToGroup(groupId, newAdminId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult RemoveAdminFromGroup(string groupId, string deletedAdminId)
        {
            List<string> memberIdList = new List<string>();
            if (GetGroupByGroupId(groupId) != null)
                memberIdList.AddRange(GetGroupByGroupId(groupId).CustomerList);
            ChatResult result = Manager.Instance.RemoveAdminFromGroup(Context.ConnectionId, groupId, deletedAdminId);
            if (result.Success)
            {
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnRemoveAdminFromGroup(groupId, deletedAdminId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult AddCustomerToGroup(string groupId, string customerId)
        {
            ChatResult result = Manager.Instance.AddCustomerToGroup(Context.ConnectionId, groupId, customerId);
            if (result.Success)
            {
                List<string> memberIdList = GetGroupByGroupId(groupId).CustomerList;
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnAddCustomerToGroup(GetGroupByGroupId(groupId), customerId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult RemoveCustomerFromGroup(string groupId, string customerId)
        {
            GroupInfo group = GetGroupByGroupId(groupId);
            List<string> memberIdList = new List<string>();
            if (group != null)
                memberIdList = new List<string>(group.CustomerList);

            ChatResult result = Manager.Instance.RemoveCustomerFromGroup(Context.ConnectionId, groupId, customerId);
            if (result.Success)
            {
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnRemoveCustomerFromGroup(groupId, customerId);
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
                    Clients.Clients(cus.ConnectionList).OnJoinGroup(GetGroupByGroupId(groupId), customerId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult LeaveGroup(string groupId)
        {
            ChatResult result = Manager.Instance.LeaveGroup(Context.ConnectionId, groupId);
            string customerId = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId).CustomerId;
            if (result.Success)
            {
                List<string> memberIdList = GetGroupByGroupId(groupId).CustomerList;
                foreach (string memberId in memberIdList)
                {
                    CustomerInfo cus = Manager.Instance.GetCustomerByCustomerId(memberId);
                    Clients.Clients(cus.ConnectionList).OnLeaveGroup(groupId, customerId);
                }
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult<List<GroupInfo>> SearchGroup(string groupName)
        {
            System.Threading.Thread.Sleep(3000);
            List<GroupInfo> groupList = Manager.Instance.SearchGroupByGroupName(Context.ConnectionId, groupName);
            if (groupList == null || groupList != null && groupList.Count == 0)
                return ChatResult<List<GroupInfo>>.CreateInstance(false, "Group not found", null);
            return ChatResult<List<GroupInfo>>.CreateInstance(true, null, groupList);
        }

        public GroupInfo GetGroupByGroupId(string groupId)
        {
            return Manager.Instance.GetGroupByGroupId(Context.ConnectionId, groupId);
        }
        #endregion

        #region Message
        [HubAuthorize]
        public List<MessageInfo> GetOfflineMessage()
        {
            List<MessageInfo> offlineMessage = Manager.Instance.GetOfflineMessage(Context.ConnectionId);
            if (offlineMessage != null)
            {
                List<MessageInfo> msgList = new List<MessageInfo>(offlineMessage);
                Manager.Instance.DeleteOfflineMessage(Context.ConnectionId);
                return msgList;
            }
            return null;
        }

        [HubAuthorize]
        public ChatResult SendMessageToFriend(MessageInfo msg)
        {
            ChatResult result = Manager.Instance.AddMessage(Context.ConnectionId, msg);
            if (result.Success)
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
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult SendMessageToGroup(MessageInfo msg)
        {
            ChatResult result = Manager.Instance.AddMessage(Context.ConnectionId, msg);
            if (result.Success)
            {
                GroupInfo group = GetGroupByGroupId(msg.ReceiverId);
                string senderId = msg.SenderId;
                List<string> customerConnectionIds = new List<string>();
                foreach (string customerId in group.CustomerList)
                    customerConnectionIds.AddRange(Manager.Instance.GetCustomerByCustomerId(customerId).ConnectionList);
                customerConnectionIds.Remove(Context.ConnectionId);
                Clients.Clients(customerConnectionIds).OnReceiveMessage(msg);
            }
            return result;
        }

        [HubAuthorize]
        public ChatResult DeleteMessage(string messageId)
        {
            ChatResult result = Manager.Instance.DeleteMessage(Context.ConnectionId, messageId);
            return result;
        }

        [HubAuthorize]
        public ChatResult<List<MessageInfo>> SearchMessage(string receiverId, DateTime fromdate, DateTime toDate)
        {
            List<MessageInfo> messageList = Manager.Instance.SearchMessage(Context.ConnectionId, receiverId, fromdate, toDate);
            if (messageList == null || messageList != null && messageList.Count <= 0)
                return ChatResult<List<MessageInfo>>.CreateInstance(false, "Message not found", null);
            return new ChatResult<List<MessageInfo>>(true, null, messageList);
        }

        [HubAuthorize]
        public ChatResult BroadcastMessage(MessageInfo message)
        {
            CustomerInfo customer = Manager.Instance.GetCustomerByConnectionId(Context.ConnectionId);
            if (customer != null)
            {
                Clients.AllExcept(Context.ConnectionId).OnReceiveMessage(message);
                return new ChatResult(true, null);
            }
            return new ChatResult(false, "Your Id doesn't exist.");
        }
        #endregion
    }
}
