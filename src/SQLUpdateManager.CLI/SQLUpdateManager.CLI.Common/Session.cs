using SQLUpdateManager.Core.Domains;
using System;

namespace SQLUpdateManager.CLI.Common
{
    public class Session
	{
        private DataServer _server;
        private Database _database;

        private readonly DateTime _appStart;

        public static Session Current { get; } = new Session();

        private Session()
        {
            _appStart = DateTime.UtcNow;
        }

        public DateTime ApplicationStartTime
        {
            get => _appStart;
        }

        public DataServer ConnectedServer
        {
            get => _server;
            set => _server = value;
        }

        public Database UsedDatabase
        {
            get => _database;
            set => _database = value;
        }
	}
}
