namespace SQLUpdateManager.CLI.Common
{
    public interface IFileManager
    {
        void Write(string path, string content);
        string Read(string path);
    }
}
