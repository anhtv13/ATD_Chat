using ATDChatClient.Manager;
using ATDChatClient.Manager.Model;
using ATDChatDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ATDChatClient.GUI
{
    public partial class SearchFriend : Form
    {
        public SearchFriend()
        {
            InitializeComponent();            
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            dgvSearchFriend.Rows.Clear();
            btnSearch.Enabled = false;
            ChatResult<List<CustomerInfo>> result = await DataManager.Instance.SearchCustomer(tbSearch.Text);
            btnSearch.Enabled = true;
            if (result.Success)
            {
                List<CustomerInfo> friendList = result.Data;
                string myCustomerId = DataManager.Instance.Customer.CustomerId;

                if (friendList.Count > 0)
                {
                    foreach (CustomerInfo cus in friendList)
                    {
                        if (cus.CustomerId != myCustomerId)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                dgvSearchFriend.Rows.Add(cus.CustomerId);
                            });
                        }
                    }
                }
            }
            else
                MessageBox.Show(result.Message);
        }

        private Image GetImageByStatus(string status)
        {
            switch (status)
            {
                case ChatStatus.Available:
                    return Properties.Resources.Available;
                case ChatStatus.Busy:
                    return Properties.Resources.Busy;
                case ChatStatus.Away:
                    return Properties.Resources.Away;
                case ChatStatus.Invisible:
                case ChatStatus.Offline:
                    return Properties.Resources.Invisible;
            }
            return Properties.Resources.Available;
        }

        private async void addFriendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatResult result = await DataManager.Instance.AddFriendRequest(contextMenuStripSearch.Tag.ToString());
            if (result.Success)
            {
                //
            }
            else
                MessageBox.Show(result.Message);
        }

        private void dgvSearchFriend_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestRow = dgvSearchFriend.HitTest(e.X, e.Y);
                if (hitTestRow.RowIndex >= 0)
                {
                    string receiverId = dgvSearchFriend.Rows[hitTestRow.RowIndex].Cells[0].Value.ToString();
                    contextMenuStripSearch.Show(dgvSearchFriend, new Point(e.X, e.Y));
                    contextMenuStripSearch.Tag = receiverId;
                }
            }
        }
    }
}
