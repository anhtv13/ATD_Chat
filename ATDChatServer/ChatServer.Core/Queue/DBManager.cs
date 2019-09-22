using ATDChatDefinition;
using Core.Controller;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ATDChatServer.Queue
{
    public class DBManager
    {
        private Queue<DBQueueObject> m_dbQueue = new Queue<DBQueueObject>();
        private Object m_dbQueueLock = new Object();
        private bool m_doWork = true;
        private int m_maxPoolMsg = 100;
        private int m_sleepPool = 50;

        private static DBManager m_instance;
        public static DBManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new DBManager();
                return m_instance;
            }
        }

        public void Start()
        {
            m_doWork = true;
            Thread queueThread = new Thread(new ThreadStart(DBManagerQueueWork));
            queueThread.Start();
        }

        public void Stop()
        {
            m_doWork = false;
            Dictionary<EnqueueType, List<DBQueueObject>> messageDict = new Dictionary<EnqueueType, List<DBQueueObject>>();
            lock (m_dbQueueLock)
            {
                int queueCount = m_dbQueue.Count;
                for (int j = 0; j < queueCount; j++)
                {
                    for (int i = 0; i < m_dbQueue.Count; i++)
                    {
                        DBQueueObject message = m_dbQueue.Dequeue();
                        if (!messageDict.ContainsKey(message.ObjectType))
                            messageDict.Add(message.ObjectType, new List<DBQueueObject>());
                        messageDict[message.ObjectType].Add(message);
                    }
                }
            }
            WriteLog(messageDict);
        }

        public void Enqueue(DBQueueObject dbQueueObject)
        {
            lock (m_dbQueueLock)
                m_dbQueue.Enqueue(dbQueueObject);
        }

        private void DBManagerQueueWork()
        {
            while (m_doWork)
            {
                Dictionary<EnqueueType, List<DBQueueObject>> messageDict = new Dictionary<EnqueueType, List<DBQueueObject>>();
                lock (m_dbQueueLock)
                {
                    while (m_dbQueue.Count > 0 && messageDict.Count < m_maxPoolMsg)
                    {
                        DBQueueObject message = m_dbQueue.Dequeue();
                        if (!messageDict.ContainsKey(message.ObjectType))
                            messageDict.Add(message.ObjectType, new List<DBQueueObject>());
                        messageDict[message.ObjectType].Add(message);
                    }
                }
                if (messageDict.Count > 0)
                    WriteLog(messageDict);
                Thread.Sleep(m_sleepPool);
            }
        }

        private void WriteLog(Dictionary<EnqueueType, List<DBQueueObject>> messageDict)
        {
            foreach (EnqueueType key in messageDict.Keys)
            {
                switch (key)
                {
                    case EnqueueType.AddCustomer:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.InsertCustomerInfo((CustomerInfo)obj.ObjData);
                            Console.WriteLine("Insert customer");
                        }
                        break;
                    case EnqueueType.UpdateCustomer:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.UpdateCustomerInfo((CustomerInfo)obj.ObjData);
                            Console.WriteLine("Update customer");
                        }
                        break;
                    case EnqueueType.ChangeLastOnline:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.ChangeLastOnline(obj.Data[0].ToString(), DateTime.Parse(obj.Data[1].ToString()));
                            Console.WriteLine("Change customer last login");
                        }
                        break;
                    case EnqueueType.AddGroup:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.InsertGroupInfo((GroupInfo)obj.ObjData);
                            Console.WriteLine("Insert group");
                        }
                        break;
                    case EnqueueType.UpdateGroup:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.UpdateGroupInfo((GroupInfo)obj.ObjData);
                            Console.WriteLine("Update group");
                        }
                        break;
                    case EnqueueType.DeleteGroup:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.DeleteGroupInfo(obj.ObjData.ToString());
                            Console.WriteLine("Delete group");
                        }
                        break;
                    case EnqueueType.JoinGroup:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.JoinGroup(obj.Data[0].ToString(), obj.Data[1].ToString());
                            Console.WriteLine("Join group");
                        }
                        break;
                    case EnqueueType.LeaveGroup:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.LeaveGroup(obj.Data[0].ToString(), obj.Data[1].ToString());
                            Console.WriteLine("Leave group");
                        }
                        break;
                    case EnqueueType.SetAdmin:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.SetAdminGroup(obj.Data[0].ToString(), obj.Data[1].ToString(), (bool)obj.Data[2]);
                            Console.WriteLine("Set Admin");
                        }
                        break;
                    case EnqueueType.InviteGroupRequest:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.AddGroupRequest((InviteGroupRequest)obj.ObjData);
                        }
                        break;
                    case EnqueueType.DeleteGroupRequest:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.DeleteGroupRequest(obj.ObjData.ToString());
                        }
                        break;
                    case EnqueueType.AddMessage:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.AddMessage((MessageInfo)obj.ObjData);
                            Console.WriteLine("Insert Message");
                        }
                        break;
                    case EnqueueType.AddFriendRequest:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.AddFriendRequest((AddFriendRequest)obj.ObjData);
                            Console.WriteLine("Add friend request");
                        }
                        break;
                    case EnqueueType.DeleteFriendRequest:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.DeleteFriendRequest(obj.ObjData.ToString());
                            Console.WriteLine("Delete friend request");
                        }
                        break;
                    case EnqueueType.AddFriend:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.AddFriend(obj.Data[0].ToString(), obj.Data[1].ToString());
                            Console.WriteLine("Add friend");
                        }
                        break;
                    case EnqueueType.Unfriend:
                        foreach (DBQueueObject obj in messageDict[key])
                        {
                            ObjectController.DataProvider.UnFriend(obj.Data[0].ToString(), obj.Data[1].ToString());
                            Console.WriteLine("Unfriend");
                        }
                        break;
                }
            }
        }
    }
}
