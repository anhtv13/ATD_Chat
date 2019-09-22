
using Core.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controller
{
    public class ObjectController
    {
        private static IDataProvider m_dataProivder = null;
        public static IDataProvider DataProvider
        {
            get
            {
                m_dataProivder = new DatabaseDataProvider();
                return m_dataProivder;
            }
        }
    }
}
