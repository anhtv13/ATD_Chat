using ATDChatDefinition;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataProvider
{
    public class Mapper
    {
        public CustomerInfo MapCustomerByID(IDataReader record)
        {
            CustomerInfo customer = new CustomerInfo();
            if (record.Read())
            {
                if (record["CustomerId"] != DBNull.Value)
                    customer.CustomerId = record["CustomerId"].ToString();
                if (record["CustomerName"] != DBNull.Value)
                    customer.CustomerName = record["CustomerName"].ToString();
                if (record["Role"] != DBNull.Value)
                    customer.Role = record["Role"].ToString();
                if (record["LastOnline"] != DBNull.Value)
                    customer.LastOnline = (DateTime)record["LastOnline"];
                return customer;
            }
            return null;
        }

        public List<CustomerInfo> mapCustomerByName(IDataReader record)
        {
            List<CustomerInfo> cusList = new List<CustomerInfo>();
            while (record.Read())
            {
                CustomerInfo customer = new CustomerInfo();
                if (record["CustomerId"] != DBNull.Value)
                    customer.CustomerId = record["CustomerId"].ToString();
                if (record["CustomerName"] != DBNull.Value)
                    customer.CustomerName = record["CustomerName"].ToString();
                if (record["Role"] != DBNull.Value)
                    customer.Role = record["Role"].ToString();
                if (record["LastOnline"] != DBNull.Value)
                    customer.LastOnline = (DateTime)record["LastOnline"];
                cusList.Add(customer);
            }
            return cusList;
        }

        public GroupInfo mapGroupByID(IDataReader record)
        {
            if (record.Read())
            {
                GroupInfo group = new GroupInfo();
                if (record["groupId"] != DBNull.Value)
                    group.GroupId = record["groupId"].ToString();
                if (record["groupName"] != DBNull.Value)
                    group.GroupName = record["groupName"].ToString();
                if (record["description"] != DBNull.Value)
                    group.Description = record["description"].ToString();
                if (record["isPrivate"] != DBNull.Value)
                {
                    int i = int.Parse(record["isPrivate"].ToString());
                    if (i == 0)
                        group.IsPrivate = false;
                    else
                        group.IsPrivate = true;
                }
                return group;
            }
            return null;
        }

        public List<GroupInfo> mapGroupByName(IDataReader record)
        {
            List<GroupInfo> groupList = new List<GroupInfo>();
            while (record.Read())
            {
                GroupInfo group = new GroupInfo();
                if (record["groupId"] != DBNull.Value)
                    group.GroupId = record["groupId"].ToString();
                if (record["groupName"] != DBNull.Value)
                    group.GroupName = record["groupName"].ToString();
                if (record["description"] != DBNull.Value)
                    group.Description = record["description"].ToString();
                if (record["isPrivate"] != DBNull.Value)
                {
                    int i = int.Parse(record["isPrivate"].ToString());
                    if (i == 0)
                        group.IsPrivate = false;
                    else
                        group.IsPrivate = true;
                }
                groupList.Add(group);
            }
            return groupList;
        }

        public List<MessageInfo> mapMessage(IDataReader record)
        {
            List<MessageInfo> messageList = new List<MessageInfo>();
            while (record.Read())
            {
                MessageInfo msg = new MessageInfo();
                if (record["messageId"] != DBNull.Value)
                    msg.Id = record["messageId"].ToString();
                if (record["senderId"] != DBNull.Value)
                    msg.SenderId = record["senderId"].ToString();
                if (record["receiverId"] != DBNull.Value)
                    msg.ReceiverId = record["receiverId"].ToString();
                if (record["content"] != DBNull.Value)
                    msg.Content = record["content"].ToString();
                if (record["datetime"] != DBNull.Value)
                    msg.Datetime = DateTime.Parse(record["datetime"].ToString());
                if (record["messagetype"] != DBNull.Value)
                    msg.MessageType = int.Parse(record["messagetype"].ToString());
                messageList.Add(msg);
            }
            return messageList;
        }

        public List<string> MapCustomerList(IDataReader record)
        {
            List<string> friendList = new List<string>();
            while (record.Read())
            {
                if (record["customerid"] != DBNull.Value)
                    friendList.Add(record["customerid"].ToString());
            }
            return friendList;
        }

        public List<string> MapGroupList(IDataReader record)
        {
            List<string> groupList = new List<string>();
            while (record.Read())
            {
                if (record["groupId"] != DBNull.Value)
                    groupList.Add(record["groupId"].ToString());
            }
            return groupList;
        }

        public List<string> MapFriendList(IDataReader record)
        {
            List<string> friendList = new List<string>();
            while (record.Read())
            {
                if (record["customerid2"] != DBNull.Value)
                    friendList.Add(record["customerid2"].ToString());
            }
            return friendList;
        }

        public Dictionary<string, InviteGroupRequest> MapGroupRequest(IDataReader record)
        {
            Dictionary<string, InviteGroupRequest> requestDict = new Dictionary<string, InviteGroupRequest>();
            while (record.Read())
            {
                InviteGroupRequest request = new InviteGroupRequest();
                if (record["Id"] != DBNull.Value)
                    request.Id = record["Id"].ToString();
                if (record["SenderId"] != DBNull.Value)
                    request.Sender = record["SenderId"].ToString();
                if (record["ReceiverId"] != DBNull.Value)
                    request.Receiver = record["ReceiverId"].ToString();
                if (record["GroupId"] != DBNull.Value)
                    request.GroupId = record["GroupId"].ToString();
                if (record["DateTime"] != DBNull.Value)
                    request.Datetime = DateTime.Parse(record["DateTime"].ToString());
                requestDict.Add(request.Id, request);
            }
            return requestDict;
        }

        public Dictionary<string, AddFriendRequest> MapFriendRequest(IDataReader record)
        {
            Dictionary<string, AddFriendRequest> requestList = new Dictionary<string, AddFriendRequest>();
            while (record.Read())
            {
                AddFriendRequest request = new AddFriendRequest();
                if (record["ID"] != DBNull.Value)
                    request.Id = record["ID"].ToString();
                if (record["SenderID"] != DBNull.Value)
                    request.Sender = record["SenderID"].ToString();
                if (record["ReceiverID"] != DBNull.Value)
                    request.Receiver = record["ReceiverID"].ToString();
                if (record["DateTime"] != DBNull.Value)
                    request.Datetime = (DateTime)record["DateTime"];
                requestList.Add(request.Id, request);
            }
            return requestList;
        }

        public List<string> MapAdminList(IDataReader record)
        {
            List<string> adminList = new List<string>();
            while (record.Read())
            {
                if (record["customerId"] != DBNull.Value)
                    adminList.Add(record["customerId"].ToString());
            }
            return adminList;
        }

        public InviteGroupRequest MapGroupRequestById(IDataReader record)
        {
            if (record.Read())
            {
                InviteGroupRequest request = new InviteGroupRequest();
                if (record["Id"] != DBNull.Value)
                    request.Id = record["Id"].ToString();
                if (record["SenderId"] != DBNull.Value)
                    request.Sender = record["SenderId"].ToString();
                if (record["ReceiverId"] != DBNull.Value)
                    request.Receiver = record["ReceiverId"].ToString();
                if (record["GroupId"] != DBNull.Value)
                    request.GroupId = record["GroupId"].ToString();
                if (record["DateTime"] != DBNull.Value)
                    request.Datetime = DateTime.Parse(record["DateTime"].ToString());
                return request;
            }
            return null;
        }

        public AddFriendRequest MapFriendRequestById(IDataReader record)
        {
            if (record.Read())
            {
                AddFriendRequest request = new AddFriendRequest();
                if (record["ID"] != DBNull.Value)
                    request.Id = record["ID"].ToString();
                if (record["SenderID"] != DBNull.Value)
                    request.Sender = record["SenderID"].ToString();
                if (record["ReceiverID"] != DBNull.Value)
                    request.Receiver = record["ReceiverID"].ToString();
                if (record["DateTime"] != DBNull.Value)
                    request.Datetime = (DateTime)record["DateTime"];
                return request;
            }
            return null;
        }
    }
}
