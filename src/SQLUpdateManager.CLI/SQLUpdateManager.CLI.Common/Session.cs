using SQLUpdateManager.Core.Domains;
using System;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Common
{
    public class SessionEntry
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Session
	{
        private DataServer _server;
        private Database _database;
        private Dictionary<string, SessionEntry> _entries = new Dictionary<string, SessionEntry>();

        private readonly DateTime _appStartLocal;
        private readonly DateTime _appStartUtc;

        public static Session Current { get; } = new Session();

        public IEnumerable<SessionEntry> Entries { get => _entries.Values; }

        private Session()
        {
            _appStartUtc = DateTime.UtcNow;
            _entries.Add("appUtcStart",
                new SessionEntry { Name = "Application start UTC time", Value = _appStartUtc.ToString() });

            _appStartLocal = DateTime.Now;
            _entries.Add("appLocalStart",
                new SessionEntry { Name = "Application start local time", Value = _appStartLocal.ToString() });
        }

        public DateTime ApplicationStartTimeLocal
        {
            get => _appStartLocal;
        }

        public DateTime ApplicationStartTimeUtc
        {
            get => _appStartUtc;
        }

        public DataServer ConnectedServer
        {
            get => _server;
            set
            {
                _server = value;

                if (value == null)
                    _entries.Remove("server");
                else
                    _entries.Add("server",
                        new SessionEntry { Name = "Current connected DBMS server", Value = $"{value.Name} {value.Location} {value.Username}" });
            }
        }

        public Database UsedDatabase
        {
            get => _database;
            set
            {
                _database = value;

                if (value == null)
                    _entries.Remove("database");
                else
                    _entries.Add("database",
                        new SessionEntry { Name = "Database in use", Value = value.Name });
            }
        }
	}
}
