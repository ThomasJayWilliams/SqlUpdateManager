using System.IO;

namespace SQLUpdateManager.Core.Internal
{
    public static class FileManager
    {
        public static string Load(string path)
        {
            using (var reader = new StreamReader(path))
                return reader.ReadToEnd();
        }

        public static void Save(string path, string data) =>
            File.AppendAllText(path, data);
    }
}
