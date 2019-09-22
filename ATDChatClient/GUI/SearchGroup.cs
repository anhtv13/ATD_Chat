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
    public partial class SearchGroup : Form
    {
        public SearchGroup()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            dgvSearchGroup.Rows.Clear();
            btnSearch.Enabled = false;
            ChatResult<List<GroupInfo>> result = await DataManager.Instance.SearchGroup(tbSearch.Text);
            btnSearch.Enabled = true;
            if (result.Success)
            {
                List<GroupInfo> groupList = result.Data;
                if (groupList.Count > 0)
                {
                    foreach (GroupInfo group in groupList)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            dgvSearchGroup.Rows.Add(group.GroupId, group.GroupName, group.CustomerList.Count);
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void dgvSearchGroup_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestRow = dgvSearchGroup.HitTest(e.X, e.Y);
                if (hitTestRow.RowIndex >= 0)
                {
                    string receiverId = dgvSearchGroup.Rows[hitTestRow.RowIndex].Cells[0].Value.ToString();
                    GroupMenuStrip.Show(dgvSearchGroup, new Point(e.X, e.Y));
                    GroupMenuStrip.Tag = receiverId;
                }
            }
        }

        private async void joinGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string groupId = (string)GroupMenuStrip.Tag;
            ChatResult result = await DataManager.Instance.JoinGroup(groupId);
            if (result.Success) { }
            else
                MessageBox.Show(result.Message);
        }
    }
}
