using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ATDChatServer
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ChatServer = new ATDChatServer();
            Application.Run(ChatServer);
        }

        public static ATDChatServer ChatServer;
    }
}
