namespace SQLUpdateManager.Core.Internal
{
    public interface IFileManager
    {
        string Load(string path);
        void Save(string content, string path);
        bool Exists(string path);
    }
}
