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
    public class ChatHub : Hub, IDisconnect
    {
        private InMemoryRepository _repository;

        public ChatHub()
        {
            _repository = InMemoryRepository.GetInstance();
        }

        #region IDisconnect and IConnected event handlers implementation

        /// <summary>
        /// Fired when a client disconnects from the system. The user associated with the client ID gets deleted from the list of currently connected users.
        /// </summary>
        /// <returns></returns>
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

        #endregion

        #region Chat event handlers

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void Send(ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                // Sanitize input
                message.Content = HttpUtility.HtmlEncode(message.Content);
                // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
                HashSet<string> extractedURLs;
                message.Content = TextParser.TransformAndExtractUrls(message.Content, out extractedURLs);
                message.Timestamp = DateTime.Now;
                Clients.onMessageReceived(message);
            }
        }

        /// <summary>
        /// Fired when a client joins the chat. Here round trip state is available and we can register the user in the list
        /// </summary>
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

        /// <summary>
        /// Invoked when a client connects. Retrieves the list of all currently connected users
        /// </summary>
        /// <returns></returns>
        public ICollection<ChatUser> GetConnectedUsers()
        {    
            return _repository.Users.ToList<ChatUser>();
        }             

        #endregion
    }
}