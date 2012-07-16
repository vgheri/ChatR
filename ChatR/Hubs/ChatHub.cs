using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;
using SignalR.Hubs;
using ChatR.Models;

namespace ChatR.Hubs
{
    public class ChatHub : Hub, IDisconnect, IConnected
    {
        #region IDisconnect and IConnected event handlers implementation

        public System.Threading.Tasks.Task Disconnect()
        {   
            return Clients.leaves(Context.ConnectionId, Caller.userName, DateTime.Now);
        }

        public System.Threading.Tasks.Task Connect()
        {
            return Clients.joined(Context.ConnectionId, Caller.username, DateTime.Now);
        }

        public System.Threading.Tasks.Task Reconnect(IEnumerable<string> groups)
        {
            return Clients.rejoined(Context.ConnectionId, Caller.userName, DateTime.Now);
        }

        #endregion

        #region Chat event handlers

        public void Send(ChatMessage message)
        {               
            message.Timestamp = DateTime.Now;
            Clients.onMessageReceived(message);
        }

        #endregion
    }
}