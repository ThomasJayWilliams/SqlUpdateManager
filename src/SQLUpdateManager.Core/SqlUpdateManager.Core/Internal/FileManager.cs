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

			if (!data.Take(8).SequenceEqual(CoreConstants.StorageMarker))
				throw new FileLoadException("Invalid file type.");

			var content = data.Skip(8).ToArray();

			if (content == null || !content.Any())
				return new byte[0];

			var decompressed = Compressor.Decompress(content);

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

			var existing = LoadBytes(path);
			var contentBytes = Encoding.UTF8.GetBytes(content);
			var compressed = Compressor.Compress(existing.Concat(contentBytes).ToArray());

			if (compressed == null || !compressed.Any())
				throw new InvalidOperationException("Error compressing content.");

			var result = CoreConstants.StorageMarker.Concat(compressed).ToArray();

			using (var stream = new FileStream(path, FileMode.Append))
				stream.Write(result, 0, result.Length);
		}

		internal static bool Exists(string path) =>
			!string.IsNullOrEmpty(path) && File.Exists(path);
    }
}
