using System.IO;

namespace SQLUpdateManager.Core.Internal
{
	internal static class FileManager
    {
		internal static string Load(string path) =>
			File.ReadAllText(path);

		internal static void Save(string path, string data) =>
            File.WriteAllText(path, data);
    }
}
