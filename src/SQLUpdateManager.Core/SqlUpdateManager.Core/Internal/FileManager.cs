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
			var data = LoadDecompressedBytes(path);

			if (data == null || !data.Any())
				return string.Empty;

			return Encoding.UTF8.GetString(data);
		}

		internal static byte[] LoadDecompressedBytes(string path)
		{
			var content = Read(path);

			if (content == null || !content.Any())
				return null;

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

			var existingData = LoadDecompressedBytes(path);

			if (existingData == null)
				existingData = new byte[0];

			var contentBytes = Encoding.UTF8.GetBytes(content);
			var concatenated = existingData.Concat(contentBytes).ToArray();
			var compressed = Compressor.Compress(concatenated);

			Write(path, compressed);
		}

		private static byte[] Read(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("File path cannot be null or empty.");

			if (Path.GetExtension(path) != CoreConstants.StorageExtension)
				throw new InvalidDataException("Invalid file extensions.");

			var data = File.ReadAllBytes(path);

			if (data == null || !data.Any())
				return null;

			if (!data.Take(8).SequenceEqual(CoreConstants.StorageMarker))
				throw new FileLoadException("Invalid file type.");

			return data.Skip(8).ToArray();
		}

		private static void Write(string path, byte[] content)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("File path cannot be null or empty.");

			if (content == null || !content.Any())
				throw new ArgumentException("Content cannot be null or empty.");

			var result = CoreConstants.StorageMarker.Concat(content).ToArray();

			using (var stream = new FileStream(path, FileMode.Append))
				stream.Write(result, 0, result.Length);
		}

		internal static bool Exists(string path) =>
			!string.IsNullOrEmpty(path) && File.Exists(path);
    }
}
