using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ATDChatClient.Manager.Model;
using ATDChatClient.Manager;

namespace ATDChatClient.GUI
{
    public partial class MemberListControl : UserControl
    {
        private string m_groupId;

        public MemberListControl()
        {
            InitializeComponent();
        }

        public void InitData(GroupInfo group, bool isAdmin)
        {
            m_groupId = group.GroupId;
            string mycustomerId = DataManager.Instance.Customer.CustomerId;

            dgvMemberList.Rows.Clear();
            if (isAdmin)
            {
                foreach (string customer in group.CustomerList)
                {
                    if (group.AdminList.Contains(customer))
                        dgvMemberList.Rows.Add(customer, GetImage(), true);
                    else
                        dgvMemberList.Rows.Add(customer, GetImage(), false);
                }
            }
            else
            {
                ColumnRemoveCustomer.Visible = false;
                ColumnAdmin.Visible = false;
                foreach (string customer in group.CustomerList)
                        dgvMemberList.Rows.Add(customer);
            }
        }

        private Image GetImage()
        {
            return Properties.Resources.Delete;
        }

        private async void dgvMemberList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)//column remove from group
            {
                string customerId = dgvMemberList.Rows[e.RowIndex].Cells[0].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Remove customer" + customerId + " ?", "Remove customer", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ChatResult result = await DataManager.Instance.RemoveCustomerFromGroup(customerId, m_groupId);
                    if (result.Success) { }
                    else
                        MessageBox.Show(result.Message);
                }
            }
            else if (e.ColumnIndex == 2)//column admin
            {
                string customerId = dgvMemberList.Rows[e.RowIndex].Cells[0].Value.ToString();
                if ((bool)dgvMemberList.Rows[e.RowIndex].Cells[2].Value == false)
                {
                    DialogResult dialogResult = MessageBox.Show("Add admin " + customerId + " ?", "Admin", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ChatResult result = await DataManager.Instance.AddAdminToGroup(customerId, m_groupId);
                        if (result.Success) { }
                        else
                            MessageBox.Show(result.Message);
                    }
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Remove admin " + customerId + " ?", "Admin", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ChatResult result = await DataManager.Instance.RemoveAdminFromGroup(customerId, m_groupId);
                        if (result.Success) { }
                        else
                            MessageBox.Show(result.Message);
                    }
                }
            }
        }

        private void dgvMemberList_MouseLeave(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
