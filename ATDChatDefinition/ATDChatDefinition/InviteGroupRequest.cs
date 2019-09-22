using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatDefinition
{
    public class InviteGroupRequest
    {
        private string m_id, m_sender, m_receiver, m_groupId, m_groupName;

        public string GroupName
        {
            get { return m_groupName; }
            set { m_groupName = value; }
        }
        private DateTime m_datetime;

        public string GroupId
        {
            get { return m_groupId; }
            set { m_groupId = value; }
        }

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

        public InviteGroupRequest(string id, string sender, string receiver, string groupId, DateTime datetime)
        {
            this.Id = id;
            this.Sender = sender;
            this.Receiver = receiver;
            this.GroupId = groupId;
            this.Datetime = datetime;
        }

        public InviteGroupRequest() { }
    }
}
