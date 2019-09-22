using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatDefinition
{
    public class AddFriendRequest
    {
        private string m_id, m_sender, m_receiver;
        private DateTime m_datetime;

        public string Receiver
        {
            get { return m_receiver; }
            set { m_receiver = value; }
        }

        public string Sender
        {
            get { return m_sender; }
            set { m_sender = value; }
        }

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }
        public DateTime Datetime
        {
            get { return m_datetime; }
            set { m_datetime = value; }
        }

        public AddFriendRequest(string id, string sender, string receiver, DateTime datetime)
        {
            this.Id = id;
            this.Sender = sender;
            this.Receiver = receiver;
            this.Datetime = datetime;
        }

        public AddFriendRequest() { }
    }
}

