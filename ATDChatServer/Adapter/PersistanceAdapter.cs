using ATDChatServer.ChatManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer.Adapter
{
    public class PersistanceAdapter
    {
        #region Group
        public void AddGroup(string groupId, string groupName, bool isPrivate)
        {
        }

        public void DeleteGroup(string groupId)
        {

        }

        public void ChangeGroupName(string groupId, string newName)
        {

        }

        public void ChangeGroupPrivate(string groupId, bool isPrivate)
        {

        }

        public void ChangeGroupDescription(string groupId, string description)
        {

        }

        public void AddAdminToGroup(string groupId, string newAdminId)
        {

        }

        public void RemoveAdminFromGroup(string groupId, string deletedAdminId)
        {

        }

        public void JoinGroup(string groupId, string customerId)
        {

        }

        public void LeaveGroup(string groupId, string myCustomerId)
        {
            
        }

        public List<GroupInfo> GetAllGroup()
        {
            return new List<GroupInfo>();
        }
        #endregion

        #region Customer
        public void AddFriendRequest(string friendCustomerId, string myCustomerId)
        {

        }

        public void DeleteFriendRequest(string frienCustomerId, string myCustomerId)
        {

        }

        public void AddFriend(string myCustomerId, string friendCustomerId)
        {

        }

        public void UnFriend(string myCustomerId, string friendCustomerId)
        {

        }

        public void ChangeCustomerName(string customerId, string newName)
        {

        }

        public void ChangeCustomerStatus(string customerId, string stt)
        {

        }

        public List<CustomerInfo> GetCustomer()
        {
            return new List<CustomerInfo>();
        }
        #endregion

        #region Message
        public void AddMessage(string myCustomerId, string messageId) { }

        public void DeleteMessage(string myCustomerId, string messageId) { }

        public void AddOfflineMessage(string myCustomerId, string messageId) { }

        public void DeleteOfflineMessage(string myCustomerId) { }
        #endregion
    }
}
