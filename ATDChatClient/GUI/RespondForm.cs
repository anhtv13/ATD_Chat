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
    public partial class RespondForm : Form
    {
        ///magic code to hide the close (X)
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 
        ///

        private int m_requestType;
        private AddFriendRequest m_friendrequest;
        private InviteGroupRequest m_grouprequest;

        public RespondForm(int requestType, object request)
        {
            InitializeComponent();
            
            m_requestType = requestType;
            if (requestType == 0)
            {
                m_friendrequest = (AddFriendRequest)request;
                string sender = m_friendrequest.Sender;
                lbText.Text = sender + " wants to add you as friend.\n Agree?";
            }
            else if (requestType == 1)
            {
                m_grouprequest = (InviteGroupRequest)request;
                string sender = m_grouprequest.Sender;
                string groupName = "";
                if (m_grouprequest.GroupName != null)
                    groupName = m_grouprequest.GroupName;
                lbText.Text = sender + " wants to add you to group +" + groupName + "+.\n Agree?";
            }
        }

        private async void btnAgree_Click(object sender, EventArgs e)
        {
            if (m_requestType == 0)
            {
                ChatResult result = await DataManager.Instance.AddFriendRespond(m_friendrequest.Id, true);
                if (!result.Success)
                    MessageBox.Show(result.Message);
            }
            else if (m_requestType == 1)
            {
                ChatResult result = await DataManager.Instance.InviteToGroupRespond(m_grouprequest.Id, true);
                if (!result.Success)
                    MessageBox.Show(result.Message);
            }
            this.Dispose();
        }

        private async void btnDecline_Click(object sender, EventArgs e)
        {
            if (m_requestType == 0)
            {
                ChatResult result = await DataManager.Instance.AddFriendRespond(m_friendrequest.Id, false);
                if (!result.Success)
                    MessageBox.Show(result.Message);
            }
            else if (m_requestType == 1)
            {
                ChatResult result = await DataManager.Instance.InviteToGroupRespond(m_grouprequest.Id, false);
                if (!result.Success)
                    MessageBox.Show(result.Message);
            }
            this.Dispose();
        }
    }
}
