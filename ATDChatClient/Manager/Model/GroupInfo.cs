using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatClient.Manager.Model
{
    public class GroupInfo
    {
        public string GroupId;
        public string GroupName;
        public bool IsPrivate;
        public string Description;
        public List<string> AdminList;
        public List<string> CustomerList;

        public GroupInfo(string adminId, string gName, bool isPrivate)
        {
            GroupId = Guid.NewGuid().ToString();
            AdminList = new List<string>();
            AdminList.Add(adminId);
            CustomerList = new List<string>();
            CustomerList.Add(adminId);
            GroupName = gName;
            IsPrivate = isPrivate;
        }

        public GroupInfo() { }
    }
}
