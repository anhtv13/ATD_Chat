using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer.ChatManager
{
    public class MessageInfo
    {
        public string Id;
        private string m_senderId;     
        private string m_receiverId;
        private DateTime m_datetime;
        private string m_content;

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
        public int MessageType;

        public MessageInfo(string messageId, string senderId, string receiverId, DateTime datetime, string content)
        {
            this.m_senderId = senderId;
            this.m_receiverId = receiverId;
            this.m_datetime = datetime;
            this.m_content = content;
        }

        public MessageInfo() { }

    }
}
