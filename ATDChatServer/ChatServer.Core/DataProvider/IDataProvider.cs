using ATDChatDefinition;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataProvider
{
    public partial interface IDataProvider
    {
        #region Friend
        bool InsertCustomerInfo(CustomerInfo cus);

        bool UpdateCustomerInfo(CustomerInfo cus);

        bool ChangeLastOnline(string customer, DateTime lastLogin);

        CustomerInfo GetCustomer(string customerId);

        List<CustomerInfo> SearchCustomer(string customerName);
        #endregion

        #region Group
        bool InsertGroupInfo(GroupInfo group);

        bool UpdateGroupInfo(GroupInfo group);

        bool DeleteGroupInfo(string groupId);

        GroupInfo GetGroup(string groupId);

        List<GroupInfo> SearchGroup(string name);

        bool AddGroupRequest(InviteGroupRequest request);

        bool DeleteGroupRequest(string requestId);        

        bool JoinGroup(string customerId, string groupId);

        bool LeaveGroup(string customerId, string groupId);

        bool SetAdminGroup(string customerId, string groupId, bool isAdmin);

        List<string> GetCustomersInGroup(string groupId);

        List<string> GetAdminsInGroup(string groupId);
        #endregion

        #region Friend
        bool AddFriendRequest(AddFriendRequest request);

        bool DeleteFriendRequest(string requestId);

        bool AddFriend(string customerId, string requestId);

        bool UnFriend(string customerId, string requestId);
        #endregion

        #region Message
        bool AddMessage(MessageInfo msg);

        List<MessageInfo> SearchMessage(string senderId, string receiverId, DateTime fromDate, DateTime toDate, int messageType);
        #endregion

        #region Get Inventory
        Dictionary<string, InviteGroupRequest> GetGroupRequest(string customerId);

        InviteGroupRequest GetGroupRequestById(string requestId);

        Dictionary<string, AddFriendRequest> GetFriendRequest(string customerId);

        AddFriendRequest GetFriendRequestById(string requestId);

        List<MessageInfo> GetOfflineMessageFriend(string customerId);

        List<MessageInfo> GetOfflineMessageGroup(string customerId, string groupId);

        List<MessageInfo> GetBroadcastMessage(string customerId);

        List<string> GetFriendList(string customerId);

        List<string> GetGroupsOfCustomer(string customerId);        
        #endregion
    }
}
