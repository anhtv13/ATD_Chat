using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel
{
    public class MessageInfo
    {
        private string m_id;        
        private string m_senderId;     
        private string m_receiverId;
        private DateTime m_datetime;
        private string m_content;
        private int m_messageType;

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

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

        public MessageInfo(string messageId, string senderId, string receiverId, DateTime datetime, string content, int messageType)
        {
            this.m_id = messageId;
            this.m_senderId = senderId;
            this.m_receiverId = receiverId;
            this.m_datetime = datetime;
            this.m_content = content;
            this.m_messageType = messageType;
        }

        public MessageInfo() { }
    }
}
