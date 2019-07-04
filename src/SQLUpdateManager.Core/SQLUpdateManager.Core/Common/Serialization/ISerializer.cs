namespace SQLUpdateManager.Core.Common
{
    public interface ISerializer
    {
        T Deserializer<T>(string data);
        string Serialize(object data);
    }
}
