using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ATDChatServer
{
    /// <summary>
    /// Class này dùng để kiểm tra client có được authorize hay không
    /// - Authorize client có thể kết nối vào server hay không (hàm AuthorizeHubConnection)
    /// - Authorize client sau khi kết nối vào server thì có thể gọi hàm [Authorize] hay không (hàm AuthorizeHubMethodInvocation)
    /// - Kiểm tra user đã đc authorize chưa (hàm UserAuthorized)
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class HubAuthorizeAttribute : AuthorizeAttribute
    {        
        /// <summary>
        /// hàm này để kiểm tra xem client có thể gọi hàm [Authorize] ở trong hub hay không
        /// - nếu như login thành công thì trả về true
        /// và hàm[Authorize] sẽ đc execute
        /// - nếu IsLogin thì trả về false
        /// và hàm đc ko execute
        /// </summary>
        /// <param name="hubIncomingInvokerContext"></param>
        /// <param name="appliesToMethod"></param>
        /// <returns></returns>
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            //Roles = "Admin";           
            bool authen = Manager.Instance.ConnectionIdAuthenticated(hubIncomingInvokerContext.Hub.Context.ConnectionId);
            return authen;
        }
    }
}
