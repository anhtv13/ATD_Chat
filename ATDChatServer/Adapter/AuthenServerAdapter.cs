using IptLib.Comm.Common;
using IptLib.IptFeeder.IptFeederCommon.Definition;
using IptLib.Utils;
using IptLib.Utils.Log4Net;
using StockVistaOrderServer.AuthenticatingClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer.Adapter
{
    public class AuthenServerAdapter
    {
        public AuthenServerAdapter()
        {
            SocketConnection.Subscribe(EventNames.ORDER_MESSAGE, OnReceiveOrderMessage);
            SocketConnection.Subscribe(EventNames.INVENTORY_RSPD, OnReceiveInventoryMessage);
        }

        //customerId -> SessionAccount -> SessionID
        private ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> m_clientSessionDict = new ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>>();

        private void OnReceiveOrderMessage(object sender, EventArgs args)
        {
            //if (sender == m_authenClient)
            {
                CommMessage message = (CommMessage)((RealtimeDataEventArgument)args).Data;
                int pos = 1;
                switch ((OrderMessageCommand)message.Data[0])
                {
                    case OrderMessageCommand.SESSIONID_UPDATE:
                        string viaKeys = SerializingHelper.DecodingString(message.Data, ref pos);
                        string customerID = SerializingHelper.DecodingString(message.Data, ref pos);
                        string sessionAccount = SerializingHelper.DecodingString(message.Data, ref pos);
                        byte[] sessionData = SerializingHelper.DecodingByteArray(message.Data, ref pos, 16);
                        if (viaKeys.Contains("ATD"))
                        {
                            if (!m_clientSessionDict.ContainsKey(customerID))
                                m_clientSessionDict.TryAdd(customerID, new ConcurrentDictionary<string, byte[]>());
                            if (!m_clientSessionDict[customerID].ContainsKey(sessionAccount))
                                m_clientSessionDict[customerID].TryAdd(sessionAccount, sessionData);
                        }
                        break;
                }
            }
        }

        private void OnReceiveInventoryMessage(object sender, EventArgs args)
        {
            CommMessage compressMessage = (CommMessage)((RealtimeDataEventArgument)args).Data;
            CommMessage message = compressMessage.ExtractZipTypeMessage();

            int pos = 0;
            int dictCount = SerializingHelper.DecodingInt32(message.Data, ref pos);
            for (int i = 0; i < dictCount; i++)
            {
                string viaKeys = SerializingHelper.DecodingString(message.Data, ref pos);

                if (viaKeys.Contains("ATD"))
                {
                    int count = SerializingHelper.DecodingInt32(message.Data, ref pos);
                    for (int j = 0; j < count; j++)
                    {
                        string custId = SerializingHelper.DecodingString(message.Data, ref pos);
                        int sCount = SerializingHelper.DecodingInt32(message.Data, ref pos);

                        m_clientSessionDict.TryAdd(custId, new ConcurrentDictionary<string, byte[]>());

                        for (int k = 0; k < sCount; k++)
                        {
                            string sAccount = SerializingHelper.DecodingString(message.Data, ref pos);
                            byte[] data = SerializingHelper.DecodingByteArray(message.Data, ref pos, 16);  //session la 16 byte

                            m_clientSessionDict[custId].TryAdd(sAccount, data);
                        }
                    }
                }
            }
        }

        public bool OnAuthenRequest(string customerId, string sessionAccount, byte[] cusId)
        {
            //if (m_clientSessionDict.ContainsKey(customerId))
            //{
            //    if (m_clientSessionDict[customerId].ContainsKey(sessionAccount))
            //    {
            //        if (m_clientSessionDict[customerId][sessionAccount] == cusId)
            //            return true;
            //    }
            //}
            //return false;
            return true;
        }
    }
}
