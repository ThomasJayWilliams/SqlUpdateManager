namespace SQLUpdateManager.Core.Domains
{
	public class DataServer : Server
	{
		public string Location { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

        public override string ToString() =>
            $"{Name} {Location} {Username}";
    }
}
