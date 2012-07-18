using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatR.Models
{
    public class InMemoryRepository
    {
        private static ICollection<ChatUser> _connectedUsers;
        private static InMemoryRepository _instance = null;

        private InMemoryRepository()
        {
            _connectedUsers = new List<ChatUser>();
        }

        public static InMemoryRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new InMemoryRepository();
            }
            return _instance;
        }

        #region Repository methods        

        public IQueryable<ChatUser> Users { get { return _connectedUsers.AsQueryable(); } }

        public void Add(ChatUser user)
        {
            _connectedUsers.Add(user);
        }

        public void Remove(ChatUser user)
        {
            _connectedUsers.Remove(user);
        }

        #endregion
    }
}