using ATDChatClient.GUI;
using ATDChatClient.Manager;
using ATDChatClient.Manager.Model;
using ATDChatDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ATDChatClient
{
    public partial class MainForm : Form
    {
        //private CustomerInfo m_customer = DataManager.Instance.Customer;

        //receiverId, ChatForm
        private Dictionary<string, ChatForm> m_chatDict = new Dictionary<string, ChatForm>();

        private Object m_lock = new Object();

        public MainForm()
        {
            InitializeComponent();
            dgvFriend.ClearSelection();
            dgvGroup.ClearSelection();

            SubscribeFromDataManager();
            ChangeStatus();
            GroupIdColumn.Visible = false;//hide groupId
            this.Text = DataManager.Instance.Customer.CustomerId;
            if (DataManager.Instance.Customer.Role != CustomerRole.Admin)
                tabControl.TabPages.Remove(tabPage3);
        }

        private void SubscribeFromDataManager()
        {
            //DataManager.Subscribe(ClientEventNames.OnGetCustomerInfo, OnGetCustomerInfo);
            DataManager.Subscribe(ClientEventNames.OnGetFriendList, OnGetFriendList);
            DataManager.Subscribe(ClientEventNames.OnGetGroupList, OnGetGroupList);
            DataManager.Subscribe(ClientEventNames.OnGetOfflineMessage, OnGetOfflineMessage);
            DataManager.Subscribe(ClientEventNames.OnGetFriendRequest, OnGetFriendRequest);
            DataManager.Subscribe(ClientEventNames.OnGetGrouprequest, OnGetGrouprequest);

            DataManager.Subscribe(ClientEventNames.OnChangeCustomerStatus, OnChangeCustomerStatus);

            DataManager.Subscribe(ClientEventNames.OnAddFriendRequest, OnAddFriendRequest);
            DataManager.Subscribe(ClientEventNames.OnAddFriendRespond, OnAddFriendRespond);
            DataManager.Subscribe(ClientEventNames.OnUnFriend, OnUnFriend);

            DataManager.Subscribe(ClientEventNames.OnAddGroup, OnAddGroup);
            DataManager.Subscribe(ClientEventNames.OnDeleteGroup, OnDeletegroup);
            DataManager.Subscribe(ClientEventNames.OnAddAdminToGroup, OnAddAdminToGroup);
            DataManager.Subscribe(ClientEventNames.OnRemoveAdminFromGroup, OnRemoveAdminFromGroup);
            DataManager.Subscribe(ClientEventNames.OnInviteToGroupRequest, OnInviteToGroupRequest);
            DataManager.Subscribe(ClientEventNames.OnInviteToGroupRespond, OnInviteToGroupRespond);
            DataManager.Subscribe(ClientEventNames.OnRemoveCustomerFromGroup, OnRemoveCustomerFromGroup);
            DataManager.Subscribe(ClientEventNames.OnJoinGroup, OnJoinGroup);
            DataManager.Subscribe(ClientEventNames.OnLeaveGroup, OnLeaveGroup);
            DataManager.Subscribe(ClientEventNames.OnUpdateGroup, OnUpdateGroup);

            DataManager.Subscribe(ClientEventNames.OnReceiveMessage, OnReceiveMessage);
        }

        private void UnSubscribeFromDataManager()
        {
            //DataManager.Subscribe(ClientEventNames.OnGetCustomerInfo, OnGetCustomerInfo);
            DataManager.Unsubscribe(ClientEventNames.OnGetFriendList, OnGetFriendList);
            DataManager.Unsubscribe(ClientEventNames.OnGetGroupList, OnGetGroupList);
            DataManager.Unsubscribe(ClientEventNames.OnGetOfflineMessage, OnGetOfflineMessage);
            DataManager.Unsubscribe(ClientEventNames.OnGetFriendRequest, OnGetFriendRequest);
            DataManager.Unsubscribe(ClientEventNames.OnGetGrouprequest, OnGetGrouprequest);

            DataManager.Unsubscribe(ClientEventNames.OnChangeCustomerStatus, OnChangeCustomerStatus);

            DataManager.Unsubscribe(ClientEventNames.OnAddFriendRequest, OnAddFriendRequest);
            DataManager.Unsubscribe(ClientEventNames.OnAddFriendRespond, OnAddFriendRespond);
            DataManager.Unsubscribe(ClientEventNames.OnUnFriend, OnUnFriend);

            DataManager.Unsubscribe(ClientEventNames.OnAddGroup, OnAddGroup);
            DataManager.Unsubscribe(ClientEventNames.OnDeleteGroup, OnDeletegroup);
            DataManager.Unsubscribe(ClientEventNames.OnAddAdminToGroup, OnAddAdminToGroup);
            DataManager.Unsubscribe(ClientEventNames.OnRemoveAdminFromGroup, OnRemoveAdminFromGroup);
            DataManager.Unsubscribe(ClientEventNames.OnInviteToGroupRequest, OnInviteToGroupRequest);
            DataManager.Unsubscribe(ClientEventNames.OnInviteToGroupRespond, OnInviteToGroupRespond);
            DataManager.Unsubscribe(ClientEventNames.OnRemoveCustomerFromGroup, OnRemoveCustomerFromGroup);
            DataManager.Unsubscribe(ClientEventNames.OnJoinGroup, OnJoinGroup);
            DataManager.Unsubscribe(ClientEventNames.OnLeaveGroup, OnLeaveGroup);
            DataManager.Unsubscribe(ClientEventNames.OnUpdateGroup, OnUpdateGroup);

            DataManager.Unsubscribe(ClientEventNames.OnReceiveMessage, OnReceiveMessage);
        }

        #region Subcribe
        private void ChangeStatus()
        {
            cbStatus.SelectedValueChanged += cbStatus_SelectedValueChanged;
            //Invoke((MethodInvoker)delegate
            //{
            string status = DataManager.Instance.Customer.Status;
            if (status.Equals(ChatStatus.Invisible))
                cbStatus.SelectedItem = ChatStatus.Invisible;
            else
                cbStatus.SelectedItem = ChatStatus.Available;
            //});            
        }

        private void OnGetFriendList(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                dgvFriend.ClearSelection();
                UpdateFriendGridView();
            });
        }

        private void OnGetGroupList(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                dgvGroup.ClearSelection();
                UpdateGroupGridView();
            });
        }

        private void OnGetOfflineMessage(object sender, EventArgs e)
        {
            List<MessageInfo> messageList = (List<MessageInfo>)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
                {
                    foreach (MessageInfo message in messageList)
                        OnReceiveMessage(message);
                });
        }

        private void OnGetFriendRequest(object sender, EventArgs e)
        {
            List<AddFriendRequest> requestList = (List<AddFriendRequest>)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
                {
                    foreach (AddFriendRequest request in requestList)
                        ExecuteAddFriendRequest(request);
                });
        }

        private void OnGetGrouprequest(object sender, EventArgs e)
        {
            List<InviteGroupRequest> requestList = (List<InviteGroupRequest>)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
            {
                foreach (InviteGroupRequest request in requestList)
                    ExecuteInviteGroupRequest(request);
            });
        }

        private void OnChangeCustomerStatus(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                string customerId = (((DataEventArgument)e)).Data[0].ToString();
                string status = (((DataEventArgument)e)).Data[1].ToString();

                if (customerId.Equals(DataManager.Instance.Customer.CustomerId))
                {
                    cbStatus.SelectedValueChanged -= cbStatus_SelectedValueChanged;
                    cbStatus.SelectedItem = status;
                    cbStatus.SelectedValueChanged += cbStatus_SelectedValueChanged;
                    //if (!status.Equals(cbStatus.SelectedItem.ToString()))
                    //    cbStatus.SelectedItem = status;
                }
                else
                {
                    foreach (DataGridViewRow dgvRow in dgvFriend.Rows)
                    {
                        if (dgvRow.Cells[1].Value.Equals(customerId))
                        {
                            dgvRow.Cells[0].Value = GetImageByStatus(status);
                            break;
                        }
                    }
                }
            });
        }

        private void OnAddFriendRequest(object sender, EventArgs e)
        {
            List<AddFriendRequest> requestList = (List<AddFriendRequest>)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
            {
                foreach (AddFriendRequest request in requestList)
                    ExecuteAddFriendRequest(request);
            });
        }

        private void OnAddFriendRespond(object sender, EventArgs e)
        {
            CustomerInfo cus = (CustomerInfo)((DataEventArgument)e).Data[0];
            bool agree = (bool)((DataEventArgument)e).Data[1];
            if (agree)
            {
                Invoke((MethodInvoker)delegate
                {
                    dgvFriend.Rows.Add(GetImageByStatus(cus.Status), cus.CustomerId);
                });
            }
            else
            {
                if (!cus.CustomerId.Equals(DataManager.Instance.Customer.CustomerId))
                    MessageBox.Show(cus.CustomerId + " doesn't accept friend request.");
            }
        }

        private void OnUnFriend(object sender, EventArgs e)
        {
            string friendId = (string)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
            {
                foreach (DataGridViewRow row in dgvFriend.Rows)
                {
                    if (row.Cells[1].Value.Equals(friendId))
                    {
                        dgvFriend.Rows.Remove(row);
                        break;
                    }
                }
            });
        }

        private void OnAddGroup(object sender, EventArgs e)
        {
            GroupInfo group = (GroupInfo)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
            {
                dgvGroup.Rows.Add(group.GroupId, group.GroupName, group.CustomerList.Count + " members");
            });

            //if (!m_chatDict.ContainsKey(group.GroupId))
            //{
            //    string customerId = DataManager.Instance.Customer.CustomerId;
            //    Invoke((MethodInvoker)delegate
            //    {
            //        CreateChatForm(customerId, group.GroupId, 200);
            //    });
            //}
        }

        private void OnDeletegroup(object sender, EventArgs e)
        {
            string groupId = ((DataEventArgument)e).Data[0].ToString();
            if (m_chatDict.ContainsKey(groupId))
            {
                m_chatDict[groupId].Dispose();
                m_chatDict.Remove(groupId);
            }
            Invoke((MethodInvoker)delegate
            {
                DataGridViewRow dgvRow = null;
                foreach (DataGridViewRow row in dgvGroup.Rows)
                {
                    if (row.Cells[0].Value.Equals(groupId))
                    {
                        dgvRow = row;
                        break;
                    }
                }
                if (dgvRow != null)
                    dgvGroup.Rows.Remove(dgvRow);
            });
        }

        private void OnAddAdminToGroup(object sender, EventArgs e)
        {
            string groupId = (string)(((DataEventArgument)e).Data[1]);

            if (m_chatDict.ContainsKey(groupId))
            {
                Invoke((MethodInvoker)delegate
                {
                    m_chatDict[groupId].GetGroupData(groupId);
                });
            }
        }

        private void OnRemoveAdminFromGroup(object sender, EventArgs e)
        {
            string groupId = (string)(((DataEventArgument)e).Data[1]);
            Invoke((MethodInvoker)delegate
            {
                m_chatDict[groupId].GetGroupData(groupId);
            });
        }

        private void OnInviteToGroupRequest(object sender, EventArgs e)
        {
            List<InviteGroupRequest> requestList = (List<InviteGroupRequest>)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
            {
                foreach (InviteGroupRequest request in requestList)
                    ExecuteInviteGroupRequest(request);
            });
        }

        private void OnInviteToGroupRespond(object sender, EventArgs e)
        {
            string customerId = (((DataEventArgument)e).Data[0]).ToString();
            GroupInfo group = (GroupInfo)(((DataEventArgument)e).Data[1]);
            bool agree = (bool)(((DataEventArgument)e).Data[2]);
            if (agree)
            {
                Invoke((MethodInvoker)delegate
                {
                    //nếu mình là ng đc add vào group
                    if (customerId.Equals(DataManager.Instance.Customer.CustomerId))
                    {
                        dgvGroup.Rows.Add(group.GroupId, group.GroupName, group.CustomerList.Count + " members");
                    }
                    else//nếu ng khác đc add vào group
                    {
                        foreach (DataGridViewRow row in dgvGroup.Rows)
                        {
                            if (row.Cells[0].Value.Equals(group))
                                row.Cells[2].Value = group.CustomerList.Count + " members";
                        }
                    }

                    //nếu cửa số chat đang mở, lấy lại dữ liệu cho nó
                    if (m_chatDict.ContainsKey(group.GroupId))
                        m_chatDict[group.GroupId].GetGroupData(group.GroupId);
                });
            }
        }

        private void OnRemoveCustomerFromGroup(object sender, EventArgs e)
        {
            string customerId = (((DataEventArgument)e).Data[0]).ToString();
            string groupId = (((DataEventArgument)e).Data[1]).ToString();
            Invoke((MethodInvoker)delegate
            {
                if (customerId.Equals(DataManager.Instance.Customer.CustomerId))
                {
                    DataGridViewRow dgvRow = null;
                    foreach (DataGridViewRow row in dgvGroup.Rows)
                    {
                        if (row.Cells[0].Value.Equals(groupId))
                        {
                            dgvRow = row;
                            break;
                        }
                    }
                    if (dgvRow != null)
                        dgvGroup.Rows.Remove(dgvRow);

                    if (m_chatDict.ContainsKey(groupId))
                        m_chatDict[groupId].Dispose();
                }
                else
                {
                    foreach (DataGridViewRow row in dgvGroup.Rows)
                    {
                        if (row.Cells[0].Value.Equals(groupId))
                            row.Cells[2].Value = DataManager.Instance.GroupDict[groupId].CustomerList.Count + " members";
                    }
                }

                if (m_chatDict.ContainsKey(groupId))
                    m_chatDict[groupId].GetGroupData(groupId);
            });
        }

        private void OnJoinGroup(object sender, EventArgs e)
        {
            string groupId = (((DataEventArgument)e).Data[1]).ToString();
            Invoke((MethodInvoker)delegate
            {
                foreach (DataGridViewRow row in dgvGroup.Rows)
                {
                    if (row.Cells[0].Value.Equals(groupId))
                        row.Cells[2].Value = DataManager.Instance.GroupDict[groupId].CustomerList.Count + " members";
                }
            });
        }

        private void OnLeaveGroup(object sender, EventArgs e)
        {
            string groupId = (((DataEventArgument)e).Data[1]).ToString();

            if (m_chatDict.ContainsKey(groupId))
            {
                m_chatDict[groupId].Dispose();
                m_chatDict.Remove(groupId);
            }
            Invoke((MethodInvoker)delegate
            {
                foreach (DataGridViewRow row in dgvGroup.Rows)
                {
                    if (row.Cells[0].Value.Equals(groupId))
                        dgvGroup.Rows.Remove(row);
                }
            });
        }

        private void OnUpdateGroup(object sender, EventArgs e)
        {
            GroupInfo group = (GroupInfo)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
                {
                    if (m_chatDict.ContainsKey(group.GroupId))
                        m_chatDict[group.GroupId].GetGroupData(group.GroupId);
                    foreach (DataGridViewRow row in dgvGroup.Rows)
                    {
                        if (row.Cells[0].Value.Equals(group.GroupId))
                        {
                            dgvGroup.Rows[row.Index].Cells[1].Value = group.GroupName;
                        }
                    }
                });
        }

        private void OnReceiveMessage(object sender, EventArgs e)
        {
            MessageInfo msg = (MessageInfo)(((DataEventArgument)e).Data[0]);
            Invoke((MethodInvoker)delegate
            {
                OnReceiveMessage(msg);
            });
        }

        private void OnReceiveMessage(MessageInfo msg)
        {
            lock (m_lock)
            {
                string receiverId = msg.ReceiverId;
                string customerId = DataManager.Instance.Customer.CustomerId;
                //receiver broadcast message
                if (msg.MessageType == 300)
                {
                    receiverId = msg.ReceiverId;
                    if (!m_chatDict.ContainsKey(receiverId))
                        CreateChatForm(customerId, receiverId, 300);
                    else
                        m_chatDict[receiverId].Show();
                }
                //receive message from member from group -> create chat from for group
                else if (msg.MessageType == 200)
                {
                    receiverId = msg.ReceiverId;
                    if (!m_chatDict.ContainsKey(receiverId))
                        CreateChatForm(customerId, receiverId, 200);
                    else
                        m_chatDict[receiverId].Show();
                }
                //receive message from an user -> create chat from for user
                else if (msg.MessageType == 100)
                {
                    receiverId = msg.SenderId;
                    if (!m_chatDict.ContainsKey(receiverId))
                        CreateChatForm(customerId, receiverId, 100);
                    else
                        m_chatDict[receiverId].Show();
                }
                m_chatDict[receiverId].ShowMessage(msg, true);
            }
        }

        private void ExecuteAddFriendRequest(AddFriendRequest request)
        {
            RespondForm addfriendForm = new RespondForm(0, request);
            addfriendForm.Show();
        }

        private void ExecuteInviteGroupRequest(InviteGroupRequest request)
        {
            RespondForm invitegroupForm = new RespondForm(1, request);
            invitegroupForm.Show();
        }
        #endregion

        private void CreateChatForm(string senderId, string receiverId, int messageType)
        {
            ChatForm chat = new ChatForm(senderId, receiverId, messageType);
            m_chatDict.Add(receiverId, chat);
            chat.Disposed += Chat_Disposed;

            m_chatDict[receiverId].Show();
        }

        private void UpdateGroupGridView()
        {
            dgvGroup.Rows.Clear();
            foreach (GroupInfo gr in DataManager.Instance.GroupDict.Values)
            {
                //if (gr.AdminList.Contains(myCustomer.CustomerId))
                //    dgvGroup.Rows.Add(gr.GroupId, gr.GroupName, gr.CustomerList.Count, gr.IsPrivate);
                //else
                //dgvGroup.Rows.Add(gr.GroupId, gr.GroupName, gr.CustomerList.Count, gr.IsPrivate);                
                dgvGroup.Rows.Add(gr.GroupId, gr.GroupName, gr.CustomerList.Count + " members");
            }
        }

        private void UpdateFriendGridView()
        {
            dgvFriend.Rows.Clear();
            foreach (CustomerInfo cus in DataManager.Instance.FriendDict.Values)
                dgvFriend.Rows.Add(GetImageByStatus(cus.Status), cus.CustomerId);//test
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

        private void Chat_Disposed(object sender, EventArgs e)
        {
            string receiverId = ((ChatForm)sender).ReceiverId;
            m_chatDict[receiverId].Dispose();
            m_chatDict.Remove(receiverId);
        }

        private async void cbStatus_SelectedValueChanged(object sender, EventArgs e)
        {
            ChatResult result = await DataManager.Instance.ChangeCustomerStatus(cbStatus.SelectedItem.ToString());
            if (result.Success)
            {
                //
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void dgvFriendList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitTestRow = dgvFriend.HitTest(e.X, e.Y);
            dgvFriend.ClearSelection();
            if (hitTestRow.RowIndex >= 0)
            {
                string receiverId = dgvFriend.Rows[hitTestRow.RowIndex].Cells[1].Value.ToString();
                if (!m_chatDict.ContainsKey(receiverId))
                {
                    string customerId = DataManager.Instance.Customer.CustomerId;
                    CreateChatForm(customerId, receiverId, 100);
                    //m_chatDict[receiverId].GetMessageHistory(receiverId, DateTime.Now.AddDays(-5), DateTime.Now, 100);
                }
            }
        }

        private void dgvGroupList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitTestRow = dgvGroup.HitTest(e.X, e.Y);
            if (hitTestRow.RowIndex >= 0)
            {
                string groupId = dgvGroup.Rows[hitTestRow.RowIndex].Cells[0].Value.ToString();
                if (!m_chatDict.ContainsKey(groupId))
                {
                    string customerId = DataManager.Instance.Customer.CustomerId;
                    CreateChatForm(customerId, groupId, 200);
                    //m_chatDict[groupId].GetMessageHistory(groupId, DateTime.Now.AddDays(-5), DateTime.Now, 200);
                }
            }
        }

        private void dgvFriendList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestRow = dgvFriend.HitTest(e.X, e.Y);
                if (hitTestRow.RowIndex >= 0)
                {
                    string receiverId = dgvFriend.Rows[hitTestRow.RowIndex].Cells[1].Value.ToString();
                    FriendMenuStrip.Show(dgvFriend, new Point(e.X, e.Y));
                    FriendMenuStrip.Tag = receiverId;
                }
            }
        }

        private void dgvGroup_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestRow = dgvGroup.HitTest(e.X, e.Y);
                if (hitTestRow.RowIndex >= 0)
                {
                    string receiverId = dgvGroup.Rows[hitTestRow.RowIndex].Cells[0].Value.ToString();
                    string customerId = DataManager.Instance.Customer.CustomerId;
                    GroupInfo group = DataManager.Instance.GroupDict[receiverId];
                    if (group.AdminList.Contains(customerId))
                        deleteGroupToolStripMenuItem.Visible = true;
                    else
                        deleteGroupToolStripMenuItem.Visible = false;
                    GroupMenuStrip.Show(dgvGroup, new Point(e.X, e.Y));
                    GroupMenuStrip.Tag = receiverId;
                }
            }
        }

        #region FriendMenuStrip
        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string receiverId = (FriendMenuStrip.Tag).ToString();
            DialogResult dialogResult = MessageBox.Show("Remove " + receiverId + " from friend list?", "Unfriend", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ChatResult result = await DataManager.Instance.UnFriend(receiverId);
                if (result.Success)
                {
                    //UpdateFriendGridView();
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
        }

        private void chatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string receiverId = (FriendMenuStrip.Tag).ToString();
            dgvFriend.ClearSelection();

            if (!m_chatDict.ContainsKey(receiverId))
            {
                string customerId = DataManager.Instance.Customer.CustomerId;
                CreateChatForm(customerId, receiverId, 100);
                //m_chatDict[receiverId].GetMessageHistory(receiverId, DateTime.Now.AddDays(-5), DateTime.Now, 100);
            }
        }

        private void inviteToGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string receiverId = (FriendMenuStrip.Tag).ToString();
        }
        #endregion

        #region GroupMenuStrip
        private void chatGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string groupId = (GroupMenuStrip.Tag).ToString();
            if (!m_chatDict.ContainsKey(groupId) || m_chatDict[groupId].IsDisposed)
            {
                string customerId = DataManager.Instance.Customer.CustomerId;
                CreateChatForm(customerId, groupId, 200);
                //m_chatDict[groupId].GetMessageHistory(groupId, DateTime.Now.AddDays(-5), DateTime.Now, 200);
            }
            Invoke((MethodInvoker)delegate
            {
                m_chatDict[groupId].Show();
            });
        }

        private async void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string groupId = (GroupMenuStrip.Tag).ToString();
            DialogResult dialogResult = MessageBox.Show("Remove " + groupId + " from group list?", "Delete Group", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ChatResult result = await DataManager.Instance.DeleteGroup(groupId);
                if (result.Success) { }
                else
                    MessageBox.Show(result.Message);
            }
        }

        private async void leaveGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string groupId = (GroupMenuStrip.Tag).ToString();
            DialogResult dialogResult = MessageBox.Show("Leave group " + groupId + " ?", "Delete Group", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ChatResult result = await DataManager.Instance.LeaveGroup(groupId);
                if (result.Success)
                {
                    //DataManager.Instance.GroupDict.Remove(groupId);
                    //UpdateGroupGridView();
                }
                else
                    MessageBox.Show(result.Message);
            }
        }

        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string groupId = (GroupMenuStrip.Tag).ToString();
        }

        private void removeCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string groupId = (GroupMenuStrip.Tag).ToString();
        }
        #endregion

        private void btnSearchFriend_Click(object sender, EventArgs e)
        {
            SearchFriend test = new SearchFriend();
            test.Show();
        }

        private async void tbCreateGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string groupName = tbCreateGroup.Text.Trim();
                if (!String.IsNullOrEmpty(groupName) && groupName.Length < 500)
                {

                    ChatResult result = await DataManager.Instance.AddGroup(groupName, false);
                    if (result.Success) { }
                    else
                        MessageBox.Show(result.Message);
                }
                else
                    MessageBox.Show("Empty string or string is too long.");
            }
        }

        private void btnSearchGroup_Click(object sender, EventArgs e)
        {
            SearchGroup searchgroupform = new SearchGroup();
            searchgroupform.Show();
        }

        private string m_groupId;
        private void dgvGroup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_groupId = dgvGroup.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private async void dgvGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string newGroupName = dgvGroup.Rows[e.RowIndex].Cells[1].Value.ToString();
                ChatResult result = await DataManager.Instance.ChangeGroupName(m_groupId, newGroupName);
                if (result.Success) { }
                else
                    MessageBox.Show(result.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnSubscribeFromDataManager();
            DataManager.Instance.DisconnectServer();
            Application.Exit();
        }

        private void btnBroadcast_Click(object sender, EventArgs e)
        {
            string receiverId = cbBroadcast.Text.Trim();
            if (!m_chatDict.ContainsKey(receiverId))
            {
                string customerId = DataManager.Instance.Customer.CustomerId;
                CreateChatForm(customerId, receiverId, 300);
                //m_chatDict[receiverId].GetMessageHistory(receiverId, DateTime.Now.AddDays(-5), DateTime.Now, 300);
            }
            Invoke((MethodInvoker)delegate
            {
                m_chatDict[receiverId].Show();
            });
        }
    }
}
