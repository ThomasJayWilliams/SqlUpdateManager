namespace SqlUpdateManager.Core
{
    public interface ISerializer
    {
        T Deserialize<T>(string data);
        string Serialize(object data);
        string Path { get; }
    }
}
