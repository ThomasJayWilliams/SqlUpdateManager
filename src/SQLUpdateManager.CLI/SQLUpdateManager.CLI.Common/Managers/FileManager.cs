using SqlUpdateManager.Core.Common;
using System.IO;

namespace SqlUpdateManager.CLI.Common
{
    public class FileManager : IFileManager
    {
        private readonly Session _session;

        public FileManager(Session session)
        {
            _session = session;
        }

        public bool Exists(string path) =>
            File.Exists(path);

        public string Load(string path)
        {
            using (var stream = new StreamReader(path))
            {
                var raw = stream.ReadToEnd();
                return raw;
            }
        }

        public void Save(string path, string content)
        {
            using (var file = new StreamWriter(path))
                file.Write(content);
        }
    }
}
