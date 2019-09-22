using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel
{
    public class GroupInfo
    {
        public string GroupId;
        public string GroupName;
        public bool IsPrivate;
        public string Description;
        public List<string> AdminList;
        public List<string> CustomerList;

        public GroupInfo(string name, bool isPrivate, string description, List<string> adminList, List<string> customerList)
        {
            GroupId = Guid.NewGuid().ToString();
            this.GroupName = name;
            this.IsPrivate = isPrivate;
            this.Description = description;
            this.AdminList = adminList;
            this.CustomerList = customerList;
        }

        public GroupInfo() { }

        #region Group
        public bool ChangeGroupName(string adminId, string newGroupName)
        {
            if (AdminList.Contains(adminId))
            {
                GroupName = newGroupName;
                return true;
            }
            return false;
        }

        public bool ChangeGroupPrivacy(string adminId, bool isPrivate)
        {
            if (AdminList.Contains(adminId))
            {
                IsPrivate = isPrivate;
                return true;
            }
            return false;
        }

        public bool ChangeGroupDescription(string adminId, string description)
        {
            if (AdminList.Contains(adminId))
            {
                if (description.Length >= 1)
                {
                    this.Description = description;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Admin
        public bool AddAdmin(string currentAdminId, string newAdminId)
        {
            if (AdminList.Contains(currentAdminId))
            {
                if (CustomerList.Contains(newAdminId))
                {
                    if (!AdminList.Contains(newAdminId))
                    {
                        AdminList.Add(newAdminId);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveAdmin(string adminId, string removeAdminId)
        {
            if (AdminList.Contains(adminId))
                return AdminList.Remove(removeAdminId);
            return false;
        }
        #endregion

        #region Customer
        public bool HasCustomer(string customerId)
        {
            if (CustomerList.Contains(customerId))
                return true;
            return false;
        }

        public bool AddCustomer(string customerId)
        {
            if (!HasCustomer(customerId))
            {
                CustomerList.Add(customerId);
                return true;
            }
            return false;
        }

        public bool RemoveCustomer(string adminId, string customerId)
        {
            if (AdminList.Contains(adminId))
                return CustomerList.Remove(customerId);
            return false;
        }

        public bool JoinGroup(string customerId)
        {
            if (!CustomerList.Contains(customerId))
            {
                CustomerList.Add(customerId);
                return true;
            }
            return false;
        }

        public bool LeaveGroup(string customerId)
        {
            return CustomerList.Remove(customerId); ;
        }

        #endregion
    }
}
