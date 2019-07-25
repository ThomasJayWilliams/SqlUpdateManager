using SQLUpdateManager.Core.Domains;
using System;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Common
{
    public class Session
	{
        private DataServer _server;
        private Database _database;
        private Dictionary<string, string> _entries = new Dictionary<string, string>();

        private readonly DateTime _appStartLocal;
        private readonly DateTime _appStartUtc;

        public static Session Current { get; } = new Session();

        public IEnumerable<string> Entries { get => _entries.Values; }

        private Session()
        {
            _appStartUtc = DateTime.UtcNow;
            _entries.Add("appStartUtc", $"Application started at {_appStartUtc.ToString()} by UTC time.");

            _appStartLocal = DateTime.Now;
            _entries.Add("appStartLocal", $"Application started at {_appStartLocal.ToString()} by local time.");
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
                    _entries.Add("server", $"Connected server: {value.Name} {value.Location}. User: {value.Username}");
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
                    _entries.Add("database", $"Database in use: {value.Name}");
            }
        }
	}
}
