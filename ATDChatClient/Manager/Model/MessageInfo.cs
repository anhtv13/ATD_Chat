using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatClient.Manager.Model
{
    public class MessageInfo
    {
        public string Id;
        private string m_senderId;
        private string m_receiverId;
        private DateTime m_datetime;
        private string m_content;
        private int m_messageType;//100: friend-friend, 200: chat group, 300: broadcast

        public string SenderId
        {
            get { return m_senderId; }
            set { m_senderId = value; }
        }
        public string ReceiverId
        {
            get { return m_receiverId; }
            set { m_receiverId = value; }
        }
        public DateTime Datetime
        {
            get { return m_datetime; }
            set { m_datetime = value; }
        }
        public string Content
        {
            get { return m_content; }
            set { m_content = value; }
        }
        public int MessageType
        {
            get { return m_messageType; }
            set { m_messageType = value; }
        }

        public MessageInfo(string msgId, string senderId, string receiverId, DateTime datetime, string content, int messageType)
        {
            this.Id = msgId;
            this.m_senderId = senderId;
            this.m_receiverId = receiverId;
            this.m_datetime = datetime;
            this.m_content = content;
            this.m_messageType = messageType;
        }     

        public MessageInfo(string senderId, string receiverId, DateTime datetime, string content, int messageType)
        {
            this.m_senderId = senderId;
            this.m_receiverId = receiverId;
            this.m_datetime = datetime;
            this.m_content = content;
            this.m_messageType = messageType;
        }        

        public MessageInfo() { }
    }
}
