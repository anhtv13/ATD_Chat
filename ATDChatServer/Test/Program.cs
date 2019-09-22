
using ATDChatDefinition;
using ATDChatServer.Queue;
using Core.Controller;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            MessageInfo msg1 = new MessageInfo("11111111111112", "1", "2", DateTime.Parse("31-AUG-2015"), "Hello", 100);
            MessageInfo msg2 = new MessageInfo("22222222222223", "1", "33333", DateTime.Parse("28-AUG-2015"), "Hello", 200);
            MessageInfo msg3 = new MessageInfo("33333333333334", "2", "1", DateTime.Parse("25-AUG-2015"), "HAHA", 100);
            MessageInfo msg4 = new MessageInfo("44444444444445", "1", "USER", DateTime.Parse("22-AUG-2015"), "Hello", 300);

            //ChatController.AddMessage(msg1);
            //ChatController.AddMessage(msg2);
            //ChatController.AddMessage(msg3);
            //ChatController.AddMessage(msg4);

            DateTime todate = Convert.ToDateTime("09-SEP-2015");
            List<MessageInfo> messageList = ChatController.SearchMessage("1", "2", DateTime.MinValue, todate, 100);
            //Dictionary<string, AddFriendRequest> dict = ChatController.GetFriendRequest("2");
            //Dictionary<string, InviteGroupRequest> dict2 = ChatController.GetGroupRequest("2");
            //List<GroupInfo> grouplist = ChatController.SearchGroup("");
            //List<CustomerInfo> cusList = ChatController.SearchCustomer("");
            //List<MessageInfo> msg = ChatController.SearchMessage("1", "2", DateTime.MinValue, DateTime.MaxValue).OrderBy(t => t.Datetime).ToList();
            DBManager manager = new DBManager();            
            Console.ReadLine();
        }
    }
}
