using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatServer.Queue
{
    public class DBQueueObject
    {
        private object m_objData;
        public object ObjData
        {
            get { return m_objData; }
            set { m_objData = value; }
        }

        private object[] m_Data;

        public object[] Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        private EnqueueType m_objectType;
        public EnqueueType ObjectType
        {
            get { return m_objectType; }
            set { m_objectType = value; }
        }

        public DBQueueObject(EnqueueType objectType, object data)
        {
            m_objectType = objectType;
            m_objData = data;
        }

        public DBQueueObject(EnqueueType objectType, object[] data)
        {
            m_objectType = objectType;
            m_Data = data;
        }
    }

    public enum EnqueueType
    {
        AddCustomer = 101,
        UpdateCustomer = 102,
        ChangeLastOnline = 103,
        AddGroup = 201,
        UpdateGroup = 202,
        DeleteGroup = 203,
        JoinGroup = 204,
        LeaveGroup = 205,
        SetAdmin = 206,
        InviteGroupRequest = 207,
        DeleteGroupRequest = 208,
        AddMessage = 301,
        AddFriendRequest = 401,
        DeleteFriendRequest = 402,
        AddFriend = 403,
        Unfriend = 404
    }
}
