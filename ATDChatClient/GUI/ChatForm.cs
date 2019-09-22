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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATDChatClient.GUI
{
    public partial class ChatForm : Form
    {
        public string MyId;
        public string MyName;
        public string ReceiverId;
        public string ReceiverName;
        private int m_messageType;
        //private AutoCompleteStringCollection source = new AutoCompleteStringCollection();

        public ChatForm(string myId, string receiverId, int messageType)
        {
            InitializeComponent();

            this.Text = receiverId;
            this.MyId = myId;
            this.ReceiverId = receiverId;
            this.ActiveControl = tbChat;
            this.m_messageType = messageType;

            if (messageType == 300)
            {
                if (myId != DataManager.Instance.Customer.CustomerId)
                    tbChat.Visible = btnSend.Visible = false;
            }
            else if (messageType == 200)
            {
                GetGroupData(receiverId);
                cbPrivateGroup.Checked = DataManager.Instance.GroupDict[receiverId].IsPrivate;
                cbPrivateGroup.CheckedChanged += cbPrivateGroup_CheckedChanged;

                ucMemberList.MouseLeave += ucMemberList_MouseLeave;
                foreach (Control ctrl in ucMemberList.Controls)
                    ctrl.MouseLeave += ucMemberList_MouseLeave;
            }

            dgvChat.KeyDown += dgvChat_KeyDown;
            DataManager.Subscribe(ClientEventNames.OnSearchMessage, OnSearchMessage);
        }

        void ucMemberList_MouseLeave(object sender, EventArgs e)
        {
            if (ucMemberList != null)
                ucMemberList.Visible = false;
        }

        private void OnSearchMessage(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }

        public void GetGroupData(string groupId)
        {
            GroupInfo group = DataManager.Instance.GroupDict[groupId];//getgroup
            this.Text = group.GroupName;//change name of chatform
            tbGroupName.Text = group.GroupName;//show groupname textbox                        
            lbMemberCount.Text = group.CustomerList.Count.ToString() + " members";
            pnTop.Visible = true;

            if (group.AdminList.Contains(MyId))
            {
                tbGroupName.Enabled = true;
                tbAddMember.Enabled = true;
                cbPrivateGroup.Visible = true;
            }
            else
            {
                tbGroupName.Enabled = false;
                tbAddMember.Enabled = false;
                cbPrivateGroup.Visible = false;
            }

            ////suggest textbox
            //List<string> friendList = DataManager.Instance.FriendDict.Keys.ToList();
            //source.AddRange(friendList.ToArray());
            //tbAddMember.AutoCompleteCustomSource = source;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void tbChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
            }
        }

        private async void SendMessage()
        {
            if (!string.IsNullOrEmpty(tbChat.Text.Trim()))
            {
                if (m_messageType == 300)
                {
                    //broadcast msg to groups

                    MessageInfo message = new MessageInfo(MyId, ReceiverId, DateTime.Now, tbChat.Text, 300);
                    ShowMessage(message, true);
                    ChatResult result = await DataManager.Instance.BroadcastMessage(message);
                }
                else if (m_messageType == 200)
                {
                    MessageInfo message = new MessageInfo(MyId, ReceiverId, DateTime.Now, tbChat.Text, 200);
                    ShowMessage(message, true);
                    ChatResult result = await DataManager.Instance.SendMessageToGroup(message);
                }

                else
                {
                    MessageInfo message = new MessageInfo(MyId, ReceiverId, DateTime.Now, tbChat.Text, 100);
                    ShowMessage(message, true);
                    ChatResult result = await DataManager.Instance.SendMessageToFriend(message);
                }
                tbChat.Text = string.Empty;
            }
        }

        public void ShowMessage(MessageInfo msg, bool newMessage)
        {
            //string message = msg.SenderId + " - " + msg.Content + " - " + msg.Datetime.ToString("hh:mm") + "\n";
            DateTime datetime = msg.Datetime;
            if (newMessage)
            {
                if (datetime.Date == DateTime.Today)
                    dgvChat.Rows.Add(msg.SenderId, msg.Content, datetime.ToShortTimeString());
                else
                    dgvChat.Rows.Add(msg.SenderId, msg.Content, datetime.ToShortDateString());
                dgvChat.FirstDisplayedScrollingRowIndex = dgvChat.RowCount - 1;
            }
            else
            {
                if (datetime.Date == DateTime.Today)
                    dgvChat.Rows.Insert(0, msg.SenderId, msg.Content, datetime.ToShortTimeString());
                else
                    dgvChat.Rows.Insert(0, msg.SenderId, msg.Content, datetime.ToShortDateString());
            }
        }

        private void rtbChat_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private async void tbAddMember_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (DataManager.Instance.GroupDict.ContainsKey(ReceiverId))
                {
                    ChatResult result = await DataManager.Instance.InviteToGroupRequest(tbAddMember.Text, ReceiverId);
                    if (result.Success) { }
                    else
                        MessageBox.Show(result.Message);
                }
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManager.Unsubscribe(ClientEventNames.OnSearchMessage, OnSearchMessage);
        }

        private void tbAddMember_MouseHover(object sender, EventArgs e)
        {
            tooltip.Show("Add new member", tbAddMember, 2000);
        }

        private void lbMemberCount_MouseHover(object sender, EventArgs e)
        {
            GroupInfo group = DataManager.Instance.GroupDict[ReceiverId];
            bool isAdmin = group.AdminList.Contains(MyId);
            ucMemberList.InitData(group, isAdmin);
            ucMemberList.Visible = true;
        }

        private async void tbGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string groupName = tbGroupName.Text.Trim();
                if (!string.IsNullOrEmpty(groupName) && groupName.Length < 100)
                {
                    DialogResult dialogResult = MessageBox.Show("Change group name?", "Group name", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ChatResult result = await DataManager.Instance.ChangeGroupName(ReceiverId, groupName);
                        if (!result.Success)
                            MessageBox.Show(result.Message);
                    }
                }
                else
                    MessageBox.Show("Empty string or string is too long.");
            }
        }

        private async void cbPrivateGroup_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Change group privacy?", "Group privacy", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ChatResult result = await DataManager.Instance.ChangeGroupPrivate(ReceiverId, !cbPrivateGroup.Checked);
                if (!result.Success)
                    MessageBox.Show(result.Message);
            }
        }

        private void tbGroupName_MouseHover(object sender, EventArgs e)
        {
            tooltip.Show("Rename group", tbGroupName, 2000);
        }
        
        //private async void dgvChat_Scroll(object sender, ScrollEventArgs e)
        //{
        //    if (dgvChat.FirstDisplayedScrollingRowIndex == 0)
        //    {
        //        ChatResult<List<MessageInfo>> result = await DataManager.Instance.SearchMessage(ReceiverId, dt.AddDays(-5), dt, m_messageType);
        //        if (result.Success)
        //        {
        //            dt = dt.AddDays(-5);
        //            List<MessageInfo> msgList = result.Data;
        //            if (msgList.Count > 0)
        //            {
        //                foreach (MessageInfo msg in msgList)
        //                    Invoke((MethodInvoker)delegate
        //                    {
        //                        ShowMessage(msg, false);
        //                    });
        //            }
        //            else
        //                dgvChat.Scroll -= dgvChat_Scroll;
        //        }

        //    }
        //}

        DateTime dt = DateTime.Now;
        public async Task<bool> GetMessageHistory(string receiverId, DateTime fromDate, DateTime toDate, int messageType)
        {
            ChatResult<List<MessageInfo>> result = await DataManager.Instance.SearchMessage(receiverId, fromDate, toDate, messageType);
            if (result.Success)
            {
                List<MessageInfo> msgList = result.Data;
                if (msgList.Count > 0)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        List<MessageInfo> newlist =  msgList.OrderByDescending(d => d.Datetime.Date).ToList();
                        foreach (MessageInfo msg in newlist)
                            ShowMessage(msg, false);
                    });
                    dt = dt.AddDays(-5);
                    return true;
                }
            }
            else
            {
                //dgvChat.Scroll -= dgvChat_Scroll;
            }
            return false;
        }

        private async void dgvChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                bool load = await GetMessageHistory(ReceiverId, dt.AddDays(-5), dt, m_messageType);
                if (!load)
                {
                    //dgvChat.KeyDown -= dgvChat_KeyDown;
                }
            }
        }
    }
}
