using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATDChatClient.Manager
{
    public class DataEventArgument:EventArgs
    {
        private object[] m_data;
        public object[] Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public DataEventArgument(object[] data)
        {
            this.Data = data;
        }
    }
}
