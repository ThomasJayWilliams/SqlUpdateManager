namespace SqlUpdateManager.Core
{
    public interface IData
    {
        string Name { get; set; }
        byte[] Hash { get; }
    }
}
