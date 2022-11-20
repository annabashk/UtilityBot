using System;
using System.Collections.Generic;
using System.Text;
using UtilityBot.Models;
using System.Collections.Concurrent;

namespace UtilityBot.Services
{
    public class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long chatId)
        {
            if (_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            var newSession = new Session();
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
