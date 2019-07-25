using System;
using System.IO;

namespace SQLUpdateManager.CLI.Common
{
    public class FileManager : IFileManager
    {
        public string Read(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            using (var stream = new StreamReader(path))
            {
                var content = stream.ReadToEnd();
                return content;
            }
        }

        public void Write(string path, string content)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content cannot be null or empty.");

            using (var file = File.OpenWrite(path))
            {
                var bytes = Session.Current.Encoding.GetBytes(content);
                file.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
