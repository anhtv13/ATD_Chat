using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel
{
    public class CustomerInfo
    {
        public string CustomerId;                
        public string CustomerName;
        public string Status;        
        public string Role;
        public DateTime LastOnline;
        public List<string> ConnectionList = new List<string>();
        public List<string> FriendList = new List<string>();
        public List<string> GroupList = new List<string>();

        public CustomerInfo() { }

        public CustomerInfo(string customerId, string customerName, string stt)
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
