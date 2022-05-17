using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SGAC.WebApp.Accesorios
{
    public class MyHub : Hub
    {
        public void SendNotifications(string strMessage, string strUserReceive)
        {
            Clients.All.receiveNotification(strMessage, strUserReceive);
        }

    }
}