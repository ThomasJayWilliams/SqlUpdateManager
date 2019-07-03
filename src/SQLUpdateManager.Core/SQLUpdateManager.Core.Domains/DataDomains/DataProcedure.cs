namespace SQLUpdateManager.Core.Domains
{
	public class DataProcedure : Procedure
	{
		public string Data { get; set; }
		public DataDatabase DataDatabase { get; set; }
    }
}
