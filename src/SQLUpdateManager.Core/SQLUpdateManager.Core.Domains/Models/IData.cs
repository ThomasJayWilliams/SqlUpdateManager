namespace SQLUpdateManager.Core.Domains
{
    public interface IData
    {
        string Name { get; set; }
        byte[] Hash { get; }
    }
}
