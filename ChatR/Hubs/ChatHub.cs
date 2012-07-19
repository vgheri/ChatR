using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;
using SignalR.Hubs;
using ChatR.Models;
using ChatR.Utilities;

namespace ChatR.Hubs
{
    public class ChatHub : Hub, IDisconnect//, IConnected
    {
        private InMemoryRepository _repository;

        public ChatHub()
        {
            _repository = InMemoryRepository.GetInstance();
        }

        #region IDisconnect and IConnected event handlers implementation

        public System.Threading.Tasks.Task Disconnect()
        {
            ChatUser user = _repository.Users.Where(u => u.Id == Context.ConnectionId).FirstOrDefault();
            if (user != null)
            {
                _repository.Remove(user);
                return Clients.leaves(Context.ConnectionId, user.Username, DateTime.Now);
            }
            return null;            
        }

        /*
        public System.Threading.Tasks.Task Connect()
        {
            return Caller.connected();            
        } */

        /*
        public System.Threading.Tasks.Task Reconnect(IEnumerable<string> groups)
        {
            return Clients.rejoined(Context.ConnectionId, Caller.userName, DateTime.Now);
        }   */

        #endregion

        #region Chat event handlers

        public void Send(ChatMessage message)
        {
            // Sanitize input: Search and replace every <script> and </script> tag with (script) and (/script)
            message.Message = HttpUtility.HtmlEncode(message.Message);
            // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
            HashSet<string> extractedURLs = new HashSet<string>();
            message.Message = TextParser.TransformAndExtractUrls(message.Message, out extractedURLs);          
            
            message.Timestamp = DateTime.Now;
            Clients.onMessageReceived(message);
        }

        public void Joined()
        {
            ChatUser user = new ChatUser()
            {
                Id = Context.ConnectionId,
                Username = Caller.username
            };
            _repository.Add(user);
            Clients.joins(Context.ConnectionId, Caller.username, DateTime.Now);
        }

        public ICollection<ChatUser> GetConnectedUsers()
        {
            ChatUser user = new ChatUser()
            {
                Id = Context.ConnectionId,
                Username = Caller.username
            };            
            return _repository.Users.ToList<ChatUser>();
        }             

        #endregion
    }
}