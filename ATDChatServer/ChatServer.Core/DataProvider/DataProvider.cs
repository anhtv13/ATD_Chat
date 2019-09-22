using ATDChatDefinition;
using IptLib.Data.DataAccess;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataProvider
{
    public partial class DatabaseDataProvider : IDataProvider
    {
        #region Friend
        public bool InsertCustomerInfo(CustomerInfo cus)
        {
            string query = "CHAT.CUSTOMER_CREATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, cus.CustomerId, cus.CustomerName, DateTime.Now, cus.Role);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool UpdateCustomerInfo(CustomerInfo cus)
        {
            string query = "CHAT.CUSTOMER_UPDATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, cus.CustomerId, cus.CustomerName);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool ChangeLastOnline(string customerId, DateTime time)
        {
            string query = "CHAT.CUSTOMER_LASTONLINE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, customerId, time);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public CustomerInfo GetCustomer(string customerId)
        {
            string query = "CHAT.CUSTOMER_GET";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            CustomerInfo cus = map.MapCustomerByID(reader);
            if (cus == null)
                return null;
            cus.FriendList = GetFriendList(customerId);
            cus.GroupList = GetGroupsOfCustomer(customerId);
            return cus;
        }

        public List<CustomerInfo> SearchCustomer(string customerName)
        {
            string query = "CHAT.CUSTOMER_SEARCH";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerName);
            Mapper map = new Mapper();
            List<CustomerInfo> cusList = map.mapCustomerByName(reader);
            return cusList;
        }

        #endregion

        #region Group
        public bool InsertGroupInfo(GroupInfo group)
        {
            string query = "CHAT.GROUP_CREATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, group.GroupId, group.GroupName, group.Description, group.IsPrivate, true);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool UpdateGroupInfo(GroupInfo group)
        {
            string query = "CHAT.GROUP_UPDATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, group.GroupId, group.GroupName, group.Description, group.IsPrivate);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool DeleteGroupInfo(string groupId)
        {
            string query = "CHAT.GROUP_DISABLE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, groupId);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public GroupInfo GetGroup(string groupId)
        {
            string query = "CHAT.GROUP_GET_BYID";
            IDataReader reader = OracleHelper.ExecuteReader(query, groupId);
            Mapper map = new Mapper();
            GroupInfo group = map.mapGroupByID(reader);
            if (group == null)
                return null;
            group.CustomerList = GetCustomersInGroup(group.GroupId);
            group.AdminList = GetAdminsInGroup(group.GroupId);
            return group;
        }

        public List<GroupInfo> SearchGroup(string name)
        {
            string query = "CHAT.GROUP_SEARCH";
            IDataReader reader = OracleHelper.ExecuteReader(query, name);
            Mapper map = new Mapper();
            List<GroupInfo> groupList = map.mapGroupByName(reader);
            if (groupList == null)
                return null;
            foreach (GroupInfo group in groupList)
            {
                group.CustomerList = GetCustomersInGroup(group.GroupId);
                group.AdminList = GetAdminsInGroup(group.GroupId);
            }
            return groupList;
        }

        public bool AddGroupRequest(InviteGroupRequest request)
        {
            string query = "CHAT.GROUPREQUEST_CREATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, request.Id, request.Sender, request.Receiver, request.GroupId, request.Datetime);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool DeleteGroupRequest(string requestId)
        {
            string query = "CHAT.GROUPREQUEST_DELETE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, requestId);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool JoinGroup(string customerId, string groupId)
        {
            string query = "CHAT.JOINGROUP";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, customerId, groupId, false);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool LeaveGroup(string customerId, string groupId)
        {
            string query = "CHAT.LEAVEGROUP";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, customerId, groupId);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool SetAdminGroup(string customerId, string groupId, bool isAdmin)
        {
            string query = "CHAT.SETADMINGROUP";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                int admin = 0;
                if (isAdmin)
                    admin = 1;
                helper.ExecuteNonQuery(query, customerId, groupId, admin);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public List<string> GetCustomersInGroup(string groupId)
        {
            string query = "CHAT.GETCUSTOMERSINGROUP";
            IDataReader reader = OracleHelper.ExecuteReader(query, groupId);
            Mapper map = new Mapper();
            return map.MapCustomerList(reader);
        }

        public List<string> GetAdminsInGroup(string groupId)
        {
            string query = "CHAT.GETADMINSINGROUP";
            IDataReader reader = OracleHelper.ExecuteReader(query, groupId);
            Mapper map = new Mapper();
            return map.MapAdminList(reader);
        }
        #endregion

        #region Add/UnFriend
        public bool AddFriendRequest(AddFriendRequest request)
        {
            string query = "CHAT.FRIENDREQUEST_CREATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, request.Id, request.Sender, request.Receiver, request.Datetime);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool DeleteFriendRequest(string requestId)
        {
            string query = "CHAT.FRIENDREQUEST_DELETE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, requestId);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool AddFriend(string customerId, string requestId)
        {
            string query = "CHAT.FRIEND_ADD";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, customerId, requestId);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public bool UnFriend(string customerId, string requestId)
        {
            string query = "CHAT.FRIEND_DELETE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, customerId, requestId);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }
        #endregion

        #region Message
        public bool AddMessage(MessageInfo msg)
        {
            string query = "CHAT.MESSAGE_CREATE";
            try
            {
                OracleDataHelper helper = OracleHelper;
                helper.BeginTransaction();
                helper.ExecuteNonQuery(query, msg.Id, msg.SenderId, msg.ReceiverId, msg.Content, msg.Datetime, msg.MessageType);
                helper.Commit();
            }
            catch (Exception ex)
            {
                OracleHelper.Rollback();
                throw ex;
            }
            return true;
        }

        public List<MessageInfo> SearchMessage(string senderId, string receiverId, DateTime fromDate, DateTime toDate, int messageType)
        {
            string query = "CHAT.MESSAGE_SEARCH";
            IDataReader reader = OracleHelper.ExecuteReader(query, senderId, receiverId, fromDate, toDate, messageType);
            Mapper map = new Mapper();
            return map.mapMessage(reader);
        }
        #endregion

        #region Get Inventory
        public Dictionary<string, InviteGroupRequest> GetGroupRequest(string customerId)
        {
            string query = "CHAT.GROUPREQUEST_GET";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            return map.MapGroupRequest(reader);
        }

        public InviteGroupRequest GetGroupRequestById(string requestId)
        {
            string query = "CHAT.GROUPREQUEST_GETBYID";
            IDataReader reader = OracleHelper.ExecuteReader(query, requestId);
            Mapper map = new Mapper();
            return map.MapGroupRequestById(reader);
        }

        public Dictionary<string, AddFriendRequest> GetFriendRequest(string customerId)
        {
            string query = "CHAT.FRIENDREQUEST_GET";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            return map.MapFriendRequest(reader);
        }

        public AddFriendRequest GetFriendRequestById(string requestId)
        {
            string query = "CHAT.FRIENDREQUEST_GETBYID";
            IDataReader reader = OracleHelper.ExecuteReader(query, requestId);
            Mapper map = new Mapper();
            return map.MapFriendRequestById(reader);
        }

        public List<MessageInfo> GetOfflineMessageFriend(string customerId)
        {
            string query = "CHAT.GETOFFLINEMESSAGEFRIEND";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            return map.mapMessage(reader);
        }

        public List<MessageInfo> GetOfflineMessageGroup(string customerId, string groupId)
        {
            string query = "CHAT.GETOFFLINEMESSAGEGROUP";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId, groupId);
            Mapper map = new Mapper();
            return map.mapMessage(reader);
        }

        public List<MessageInfo> GetBroadcastMessage(string customerId)
        {
            string query = "CHAT.GETBROADCASTMESSAGE";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            return map.mapMessage(reader);
        }

        public List<string> GetFriendList(string customerId)
        {
            string query = "CHAT.FRIEND_GET";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            return map.MapFriendList(reader);
        }

        public List<string> GetGroupsOfCustomer(string customerId)
        {
            string query = "CHAT.GETGROUPSOFCUSTOMER";
            IDataReader reader = OracleHelper.ExecuteReader(query, customerId);
            Mapper map = new Mapper();
            return map.MapGroupList(reader);
        }
        #endregion
    }
}
