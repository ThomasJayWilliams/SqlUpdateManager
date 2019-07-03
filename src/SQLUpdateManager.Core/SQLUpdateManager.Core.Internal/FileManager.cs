using System.IO;

namespace SQLUpdateManager.Core.Internal
{
    public static class FileManager
    {
		public static string Load(string path) =>
			File.ReadAllText(path);

        public static void Save(string path, string data) =>
            File.WriteAllText(path, data);
    }
}
