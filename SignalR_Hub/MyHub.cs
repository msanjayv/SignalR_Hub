using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR_Hub
{
    public class MyHub : Hub
    {
        static List<string> connectionIds = new List<string>();

        public Task SendMsg()
        {
             return Clients.All.SendAsync("MsgFromServer", DateTime.Now.ToLongTimeString());
        }

        public Task SendMsgToOthers(string message)
        {
            return Clients.Others.SendAsync("MsgFromServer", message);
        }

        public Task SendMsgToClient(string connectionId, string message)
        {
            return Clients.Client(connectionId).SendAsync("MsgFromServer", message);
        }

        public override Task OnConnectedAsync()
        {
            if(!connectionIds.Contains(Context.ConnectionId))
                connectionIds.Add(Context.ConnectionId);
            Clients.All.SendAsync("userConnected", connectionIds);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            connectionIds.Remove(Context.ConnectionId);
            Clients.All.SendAsync("userDisConnected", connectionIds);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class user : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }

}
