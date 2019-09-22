
using ATDChatServer.Adapter;
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
        private string m_url;
            //= "http://localhost:8080/";
        private AuthenServerAdapter m_authenServerAdapter;
        private IDisposable SignalR;

        public ATDChatServer()
        {
            InitializeComponent();
            m_authenServerAdapter = new AuthenServerAdapter(); //Authen
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            m_url = tbServer.Text.Trim() + ":" + tbPort.Text.Trim();
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
                Disconnect();
                rtbNotification.AppendText("Server stops.\n");
                btnStopServer.Enabled = false;
                btnStartServer.Enabled = true;
            }
        }

        private void Disconnect()
        {
            SignalR.Dispose();
            Manager.Instance.StopAdapter();
        }

        private void ATDChatServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SignalR != null)
                Disconnect();
        }
    }
}
