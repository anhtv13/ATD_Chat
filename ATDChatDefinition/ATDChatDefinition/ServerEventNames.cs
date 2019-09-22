using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatDefinition
{
    public class ServerEventNames
    {
        //authen
        public const string AuthenRequest = "AuthenRequest";

        //customer
        public const string ChangeCustomerStatus = "ChangeCustomerStatus";
        public const string ChangeCustomerName = "ChangeCustomerName";
        public const string GetCustomerInfo = "GetCustomerInfo";
        public const string GetInventory = "GetInventory";

        //friends
        public const string AddFriendRequest = "AddFriendRequest";
        public const string AddFriendRespond = "AddFriendRespond";
        public const string UnFriend = "UnFriend";
        public const string SearchCustomer = "SearchCustomer";

        //message
        public const string SendMessageToFriend = "SendMessageToFriend";
        public const string SendMessageToGroup = "SendMessageToGroup";
        public const string DeleteMessage = "DeleteMessage";
        public const string BroadcastMessage = "BroadcastMessage";
        public const string SearchMessage = "SearchMessage";

        //group
        public const string AddGroup = "AddGroup";
        public const string DeleteGroup = "DeleteGroup";
        public const string ChangeGroupName = "ChangeGroupName";
        public const string ChangeGroupPrivacy = "ChangeGroupPrivacy";
        public const string ChangeGroupDescription = "ChangeGroupDescription";
        public const string AddAdminToGroup = "AddAdminToGroup";
        public const string RemoveAdminFromGroup = "RemoveAdminFromGroup";
        public const string InviteToGroupRequest = "InviteToGroupRequest";
        public const string InviteToGroupRespond = "InviteToGroupRespond";
        public const string RemoveCustomerFromGroup = "RemoveCustomerFromGroup";
        public const string JoinGroup = "JoinGroup";
        public const string LeaveGroup = "LeaveGroup";
        public const string SearchGroup = "SearchGroup";
    }
}
