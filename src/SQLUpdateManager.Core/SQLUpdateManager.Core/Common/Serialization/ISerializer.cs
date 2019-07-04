namespace SQLUpdateManager.Core.Common
{
    public interface ISerializer
    {
        T Deserialize<T>(string data);
        string Serialize(object data);
    }
}
