using ATDChatDefinition;
using ATDChatServer.ChatManager;
using ATDChatServer.Queue;
using Core.Controller;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer.Adapter
{
    public class PersistanceAdapter
    {
        public PersistanceAdapter()
        {
            DBManager.Instance.Start();
        }

        public void StopAdapter()
        {
            DBManager.Instance.Stop();
        }

        #region Friend
        public void AddCustomer(CustomerInfo customer)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.AddCustomer, customer));
        }

        public void AddFriendRequest(AddFriendRequest request)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.AddFriendRequest, request ));
        }

        public void DeleteFriendRequest(string requestId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.DeleteFriendRequest, requestId ));
        }

        public void AddFriend(string myCustomerId, string friendCustomerId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.AddFriend, new object[] { myCustomerId, friendCustomerId }));
        }

        public void UnFriend(string myCustomerId, string friendCustomerId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.Unfriend, new object[] { myCustomerId, friendCustomerId }));
        }

        public AddFriendRequest GetFriendRequestById(string requestId)
        {
            return ChatController.GetFriendRequestById(requestId);
        }

        public void UpdateCustomer(CustomerInfo customer)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.UpdateCustomer, customer));
        }

        public void ChangeLastOnline(string customerId, DateTime time)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.ChangeLastOnline, new object[] { customerId, time }));
        }

        public CustomerInfo GetCustomer(string customerId)
        {
            CustomerInfo customer = ChatController.GetCustomer(customerId); //get data from DB
            if (customer != null)
                return customer;
            return null;
        }

        public List<CustomerInfo> SearchCustomer(string customerName)
        {
            List<CustomerInfo> customerList = ChatController.SearchCustomer(customerName);
            if (customerList != null)
                return customerList;
            return null;
        }

        public Dictionary<string, AddFriendRequest> GetFriendRequest(string customerId)
        {
            return ChatController.GetFriendRequest(customerId);
        }
        #endregion

        #region Group
        public void AddGroup(GroupInfo group)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.AddGroup, group));
        }

        public void DeleteGroup(string groupId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.DeleteGroup, groupId));
        }

        public void UpdateGroup(GroupInfo group)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.UpdateGroup, group));
        }

        public void SetAdmin(string customerId, string groupId, bool isAdmin)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.SetAdmin, new object[] { customerId, groupId, isAdmin }));
        }

        public void JoinGroup(string customerId, string groupId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.JoinGroup, new object[] { customerId, groupId }));
        }

        public void LeaveGroup(string customerId, string groupId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.LeaveGroup, new object[] { customerId, groupId }));
        }

        public GroupInfo GetGroup(string groupId)
        {
            GroupInfo group = ChatController.GetGroup(groupId);
            if (group == null)
                return null;
            return group;
        }

        public List<GroupInfo> SearchGroup(string groupName)
        {
            List<GroupInfo> groupList = ChatController.SearchGroup(groupName);
            if (groupList == null)
                return null;
            return groupList;
        }

        public void InviteToGroupRequest(InviteGroupRequest request)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.InviteGroupRequest, request));
        }

        public void DeleteGroupRequest(string requestId)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.DeleteGroupRequest, requestId));
        }

        public Dictionary<string, InviteGroupRequest> GetGroupRequest(string customerId)
        {
            return ChatController.GetGroupRequest(customerId);
        }

        public InviteGroupRequest GetGroupRequestById(string requestId)
        {
            return ChatController.GetGroupRequestByID(requestId);
        }
        #endregion

        #region Message
        public void AddMessage(MessageInfo message)
        {
            DBManager.Instance.Enqueue(new DBQueueObject(EnqueueType.AddMessage, message));
        }

        public List<MessageInfo> GetOfflineMessageFriend(string customerId)
        {
            List<MessageInfo> messageList = ChatController.GetOfflineMessageFriend(customerId);
            if (messageList == null)
                return null;
            return messageList;
        }

        public List<MessageInfo> GetOfflineMessageGroup(string customerId, string groupId)
        {
            List<MessageInfo> messageList = ChatController.GetOfflineMessageGroup(customerId, groupId);
            if (messageList == null)
                return null;
            return messageList;
        }

        public List<MessageInfo> GetBroadcastMessage(string customerId)
        {
            List<MessageInfo> messageList = ChatController.GetBroadcastMessage(customerId);
            if (messageList == null)
                return null;
            return messageList;
        }

        public List<MessageInfo> SearchMessage(string senderId, string receiverId, DateTime fromDate, DateTime toDate, int messageType)
        {
            List<MessageInfo> messageList = ChatController.SearchMessage(senderId, receiverId, fromDate, toDate, messageType);
            if (messageList == null)
                return null;
            return messageList;
        }
        #endregion               
    }
}
