using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATDChatServer.ChatManager;
using ATDChatDefinition;

namespace ATDChatServer.ChatManager
{
    public class CustomerInfo
    {
        public string CustomerId;
        public List<string> ConnectionList;
        public string Status;
        public string CustomerName;
        public List<string> FriendList;
        public List<string> GroupList;
        public bool isAdmin;

        public CustomerInfo() { }

        public CustomerInfo(string customerId)
        {
            CustomerId = customerId;
            Status = ChatDefinition.Available;
            ConnectionList = new List<string>();
            FriendList = new List<string>();
            GroupList = new List<string>();
        }

        public CustomerInfo(String customerId, string customerName, string stt)
        {
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.Status = stt;
        }

        public void ChangeOnlineStatus(string stt)
        {
            Status = stt;
        }

        public bool ChangeCustomerName(string newName)
        {
            if (newName.Length >= 5)
            {
                CustomerName = newName;
                return true;
            }
            return false;
        }

        #region ConnectionId
        public void AddNewConnectionID(string connectionId)
        {
            if (!ConnectionList.Contains(connectionId))
                ConnectionList.Add(connectionId);
        }

        public void RemoveConnectionID(string connectionId)
        {
                ConnectionList.Remove(connectionId);
        }

        public bool ContainsConnectionId(string connectionId)
        {
            return ConnectionList.Contains(connectionId);
        }
        #endregion

        #region FriendList
        public bool AddNewFriend(string friendId)
        {
            if (!FriendList.Contains(friendId))
            {
                FriendList.Add(friendId);
                return true;
            }
            return false;
        }

        public bool UnFriend(string friendId)
        {
            return FriendList.Remove(friendId);
        }

        public List<string> GetFriendList()
        {
            return FriendList;
        }
        #endregion    

        #region Group

        public bool IsInGroup(string groupId)
        {
            if (!GroupList.Contains(groupId))
                return false;
            return true;
        }

        public bool JoinGroup(string groupId)
        {
            if (!IsInGroup(groupId))
            {
                GroupList.Add(groupId);
                return true;
            }
            return false;
        }

        public bool LeaveGroup(string groupId)
        {
            return GroupList.Remove(groupId); 
        }
        #endregion
        
    }
}
