using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatR.Models
{
    public class InMemoryRepository
    {
        private static ICollection<ChatUser> _connectedUsers;
        private static InMemoryRepository _instance = null;
        private static int seedCounter;

        private InMemoryRepository()
        {
            _connectedUsers = new List<ChatUser>();
            seedCounter = 0;
        }

        public static InMemoryRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new InMemoryRepository();
            }
            return _instance;
        }

        #region Private methods

        private static int GetNextSeedCounter()
        {
            seedCounter++;
            return seedCounter;
        }

        #endregion

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

        public string GetRandomizedUsername(string username)
        {
            return username + "_" + GetNextSeedCounter().ToString();
        }

        #endregion
    }
}