using ATDChatClient.GUI;
using ATDChatClient.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ATDChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            DataManager.Subscribe("Login", OnLogin);
        }

        private void OnLogin(object sender, EventArgs e)
        {
            bool login = (bool)((DataEventArgument)e).Data[0];
            string errorMessage = ((DataEventArgument)e).Data[1].ToString();

            Invoke((MethodInvoker)delegate
            {
                if (login == false)
                {
                    btnLogin.Enabled = true;
                    lbNotification.Text = errorMessage;
                }
                else
                {
                    this.Hide();
                    MainForm mainform = new MainForm();
                    DataManager.Instance.GetInventory();
                    mainform.Show();
                    
                }
            });
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            DataManager.Instance.ConnectServer(tbServer.Text, tbPort.Text, tbCustomerId.Text, tbAccountId.Text);
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
