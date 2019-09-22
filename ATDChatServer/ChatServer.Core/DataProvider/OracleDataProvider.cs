using IptLib.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataProvider
{
    public partial class DatabaseDataProvider : IDataProvider
    {
        private OracleDataHelper m_OracleHelper;

        private OracleDataHelper OracleHelper
        {
            get
            {
                string s = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.25.1.111)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=fdc)));User Id=vnds;Password=vnds;Max Pool Size=200;";
                m_OracleHelper = new OracleDataHelper(s);
                return m_OracleHelper;
            }
        }

        private SqlDataHelper m_SQLHelper;
        private SqlDataHelper SQLHelper(string connectionStringName)
        {
            m_SQLHelper = new SqlDataHelper(connectionStringName);
            return m_SQLHelper;
        }

        private const string FDCOracleConnection = "FDCOracleConnection";
        private const string StockVistaSQLConnection = "StockVistaSQLConnection";
    }
}
