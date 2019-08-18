namespace SqlUpdateManager.Core.Common
{
    public interface IData
    {
        string Name { get; set; }
        byte[] Hash { get; }
    }
}
