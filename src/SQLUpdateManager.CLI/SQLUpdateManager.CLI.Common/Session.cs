using SQLUpdateManager.Core.Domains;
using System;
using System.Text;

namespace SQLUpdateManager.CLI.Common
{
    public class Session
	{
        public Session()
        {
            ApplicationStartTimeUtc = DateTime.UtcNow;
            ApplicationStartTimeLocal = DateTime.Now;
        }

        public DateTime ApplicationStartTimeLocal { get; }
        public DateTime ApplicationStartTimeUtc { get; }

        public Encoding Encoding { get; set; }
        public DataServer ConnectedServer { get; set; }
        public Database UsedDatabase { get; set; }
        public Storage Storage { get; set; }
    }
}
