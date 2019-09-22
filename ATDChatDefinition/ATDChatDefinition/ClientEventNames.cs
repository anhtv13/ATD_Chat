using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatDefinition
{
    public class ClientEventNames
    {
        //customer
        public const string OnGetInventory = "OnGetInventory";
        public const string OnGetCustomerInfo = "OnGetCustomerInfo";
        public const string OnGetFriendList = "OnGetFriendList";
        public const string OnGetGroupList = "OnGetGroupList";
        public const string OnGetOfflineMessage = "OnGetOfflineMessage";
        public const string OnGetFriendRequest = "OnGetFriendRequest";
        public const string OnGetGrouprequest = "OnGetGrouprequest";

        public const string OnChangeCustomerStatus = "OnChangeCustomerStatus";
        public const string OnChangeCustomerName = "OnChangeCustomerName";
        public const string OnSearchCustomer = "OnSearchCustomer";
        public const string OnSearchGroup = "OnSearchGroup";
        public const string OnSearchMessage = "OnSearchMessage";

        //friend
        public const string OnAddFriendRequest = "OnAddFriendRequest";
        public const string OnAddFriendRespond = "OnAddFriendRespond";
        public const string OnUnFriend = "OnUnFriend";
        
        //message
        public const string OnReceiveMessage = "OnReceiveMessage";

        //group
        public const string OnAddGroup = "OnAddGroup";
        public const string OnDeleteGroup = "OnDeleteGroup";
        public const string OnChangeGroupName = "OnChangeGroupName";
        public const string OnChangeGroupPrivacy = "OnChangeGroupPrivacy";
        public const string OnChangeGroupDescription = "OnChangeGroupDescription";
        public const string OnAddAdminToGroup = "OnAddAdminToGroup";
        public const string OnRemoveAdminFromGroup = "OnRemoveAdminFromGroup";
        public const string OnInviteToGroupRequest = "OnInviteToGroupRequest";
        public const string OnInviteToGroupRespond = "OnInviteToGroupRespond";
        public const string OnRemoveCustomerFromGroup = "OnRemoveCustomerFromGroup";
        public const string OnJoinGroup = "OnJoinGroup";
        public const string OnLeaveGroup = "OnLeaveGroup";
        public const string OnUpdateGroup = "OnUpdateGroup";
        public const string OnSearchGroupByGroupName = "OnSearchGroupByGroupName";
    }
}
