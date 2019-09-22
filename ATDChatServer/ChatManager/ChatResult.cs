using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer.ChatManager
{
    public class ChatResult<T>
    {
        private bool m_success;
        private string m_message;
        private T m_data;

        public T Data
        {
            get { return m_data; }
            set { m_data = value; }
        }
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }
        public bool Success
        {
            get { return m_success; }
            set { m_success = value; }
        }

        public static ChatResult<T> CreateInstance(bool success, string message, T data)
        {
            return new ChatResult<T>(success, message, data);
        }

        public ChatResult(bool success, string message, T data)
        {
            // TODO: Complete member initialization
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }
    }

    public class ChatResult
    {
        private bool m_success;
        private string m_message;

        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }
        public bool Success
        {
            get { return m_success; }
            set { m_success = value; }
        }
        public ChatResult(bool success, string message)
        {
            // TODO: Complete member initialization
            this.Success = success;
            this.Message = message;
        }

        public ChatResult() { }
    }
}