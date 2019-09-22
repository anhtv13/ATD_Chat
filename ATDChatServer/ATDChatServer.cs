using ATDChatServer.Adapter;
using ATDChatServer.ChatManager;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATDChatServer
{
    public partial class ATDChatServer : Form
    {
        private string m_url = "http://localhost:8080/";
        private AuthenServerAdapter m_authenServerAdapter;
        private Manager m_manager;
        private IDisposable SignalR;

        public ATDChatServer()
        {
            InitializeComponent();

            m_authenServerAdapter = new AuthenServerAdapter(); //Authen
            m_manager = Manager.Instance;
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;
            try
            {
                SignalR = WebApp.Start(m_url);
                rtbNotification.AppendText("Server starts at " + m_url + "\n");
            }
            catch (Exception ex)
            {
                rtbNotification.AppendText(ex + "\n Fail to start server.\n");
            }
        }

        public bool OnAuthenRequest(string customerId, string sessionAccount, byte[] cusId)
        {
            return m_authenServerAdapter.OnAuthenRequest(customerId, sessionAccount, cusId);
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (SignalR != null)
            {
                SignalR.Dispose();
                rtbNotification.AppendText("Server stops.\n");
                btnStopServer.Enabled = false;
                btnStartServer.Enabled = true;
            }
        }  
    }
}
