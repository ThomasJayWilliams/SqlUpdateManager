using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlUpdateManager.Core
{
    internal static class FileManager
    {
		internal static string Load(string path)
		{
			var data = LoadBytes(path);

			if (data == null || !data.Any())
				return string.Empty;

			return Encoding.UTF8.GetString(data);
		}

		internal static byte[] LoadBytes(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("File path cannot be null or empty.");

			if (Path.GetExtension(path) != CoreConstants.StorageExtension)
				throw new InvalidDataException("Invalid file extensions.");

			var data = File.ReadAllBytes(path);

			if (data == null || !data.Any())
				return new byte[0];

			for (int i = 0; i < 8; i++)
			{
				if (data[i] != CoreConstants.StorageMarker[i])
					throw new FileLoadException("Invalid file data.");
			}

			var content = data.Skip(8).ToArray();

			if (content == null || !content.Any())
				return new byte[0];

			var decompressed = Compressor.Decompress(Encoding.UTF8.GetString(content));

			return decompressed;
		}

		internal static void Save(string content, string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("File path cannot be null or empty.");

			if (string.IsNullOrEmpty(content))
				throw new ArgumentException("Content cannot be null or empty.");

			if (!Exists(path))
				throw new FileNotFoundException($"{path} file does not exist.");

			var existing = Load(path);
			var compressed = Compressor.Compress(existing + content);
			var marked = CoreConstants.StorageMarker.Concat(compressed).ToArray();

			File.WriteAllBytes(path, marked);
		}

		internal static bool Exists(string path) =>
			!string.IsNullOrEmpty(path) && File.Exists(path);
    }
}
