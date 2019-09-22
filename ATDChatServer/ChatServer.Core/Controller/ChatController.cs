using ATDChatDefinition;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controller
{
    public class ChatController
    {
        public static bool InsertCustomerInfo(CustomerInfo cus)
        {
            return ObjectController.DataProvider.InsertCustomerInfo(cus);
        }

        public static CustomerInfo GetCustomer(string customerId)
        {
            return ObjectController.DataProvider.GetCustomer(customerId);
        }

        public static List<CustomerInfo> SearchCustomer(string customerName)
        {
            return ObjectController.DataProvider.SearchCustomer(customerName);
        }

        public static bool InsertGroupInfo(GroupInfo group)
        {
            return ObjectController.DataProvider.InsertGroupInfo(group);
        }

        public static bool UpdateGroupInfo(GroupInfo group)
        {
            return ObjectController.DataProvider.UpdateGroupInfo(group);
        }

        public static bool DeleteGroupInfo(string groupId)
        {
            return ObjectController.DataProvider.DeleteGroupInfo(groupId);
        }

        public static GroupInfo GetGroup(string groupId)
        {
            return ObjectController.DataProvider.GetGroup(groupId);
        }

        public static List<GroupInfo> SearchGroup(string groupName)
        {
            return ObjectController.DataProvider.SearchGroup(groupName);
        }

        public static bool JoinGroup(string customerId, string groupId)
        {
            return ObjectController.DataProvider.JoinGroup(customerId, groupId);
        }

        public static bool LeaveGroup(string customerId, string groupId)
        {
            return ObjectController.DataProvider.LeaveGroup(customerId, groupId);
        }

        public static bool SetAdmin(string customerId, string groupId, bool isAdmin)
        {
            return ObjectController.DataProvider.SetAdminGroup(customerId, groupId, isAdmin);
        }

        public static bool AddGroupRequest(InviteGroupRequest request)
        {
            return ObjectController.DataProvider.AddGroupRequest(request);
        }

        public static bool DeleteGroupRequest(string requestId)
        {
            return ObjectController.DataProvider.DeleteGroupRequest(requestId);
        }

        public static List<string> GetCustomersInGroup(string groupId)
        {
            return ObjectController.DataProvider.GetCustomersInGroup(groupId);
        }

        public static List<string> GetAdminInGroup(string groupId)
        {
            return ObjectController.DataProvider.GetAdminsInGroup(groupId);
        }

        public static bool AddFriendRequest(AddFriendRequest requestId)
        {
            return ObjectController.DataProvider.AddFriendRequest(requestId);
        }

        public static bool DeleteFriendRequest(string requestId)
        {
            return ObjectController.DataProvider.DeleteFriendRequest(requestId);
        }

        public static bool AddFriend(string customerId, string requestId)
        {
            return ObjectController.DataProvider.AddFriend(customerId, requestId);
        }

        public static bool UnFriend(string customerId, string requestId)
        {
            return ObjectController.DataProvider.UnFriend(customerId, requestId);
        }

        public static bool AddMessage(MessageInfo msg)
        {
            return ObjectController.DataProvider.AddMessage(msg);
        }

        public static List<MessageInfo> SearchMessage(string senderId, string receiverId, DateTime fromDate, DateTime toDate, int messageType)
        {
            return ObjectController.DataProvider.SearchMessage(senderId, receiverId, fromDate, toDate, messageType);
        }

        public static Dictionary<string, InviteGroupRequest> GetGroupRequest(string customerId)
        {
            return ObjectController.DataProvider.GetGroupRequest(customerId);
        }

        public static InviteGroupRequest GetGroupRequestByID(string requestId)
        {
            return ObjectController.DataProvider.GetGroupRequestById(requestId);
        }

        public static Dictionary<string, AddFriendRequest> GetFriendRequest(string customerId)
        {
            return ObjectController.DataProvider.GetFriendRequest(customerId);
        }

        public static AddFriendRequest GetFriendRequestById(string requestId)
        {
            return ObjectController.DataProvider.GetFriendRequestById(requestId);
        }

        public static List<MessageInfo> GetOfflineMessageFriend(string customerId)
        {
            return ObjectController.DataProvider.GetOfflineMessageFriend(customerId);
        }

        public static List<MessageInfo> GetOfflineMessageGroup(string customerId, string groupId)
        {
            return ObjectController.DataProvider.GetOfflineMessageGroup(customerId, groupId);
        }

        public static List<MessageInfo> GetBroadcastMessage(string customerId)
        {
            return ObjectController.DataProvider.GetBroadcastMessage(customerId);
        }

        public static List<string> GetFriendList(string customerId)
        {
            return ObjectController.DataProvider.GetFriendList(customerId);
        }

        public static List<string> GetGroupsOfCustomer(string customerId)
        {
            return ObjectController.DataProvider.GetGroupsOfCustomer(customerId);
        }        
    }
}
