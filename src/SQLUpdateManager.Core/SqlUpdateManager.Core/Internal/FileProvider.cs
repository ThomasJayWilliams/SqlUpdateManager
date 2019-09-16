using SqlUpdateManager.Core.Data;
using System;
using System.IO;
using System.Linq;

namespace SqlUpdateManager.Core
{
	internal static class FileProvider
    {
		internal static T Load<T>(string path)
		{
			var data = LoadBytes(path);

			if (data == null || !data.Any())
				return default(T);

			var serializer = new BsonSerializer();

			return serializer.Deserialize<T>(data);
		}

		internal static byte[] LoadBytes(string path)
		{
			var content = Read(path);

			if (content == null || !content.Any())
				return null;

			return content;
		}

		internal static void Save(object content, string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("File path cannot be null or empty.");

			if (content == null)
				throw new ArgumentException("Content cannot be null.");

			if (!Exists(path))
				throw new FileNotFoundException($"{path} file does not exist.");

			var serializer = new BsonSerializer();
			var contentBytes = serializer.Serialize(content);

			Write(path, contentBytes);
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

			var result = content;
			var fileInfo = new FileInfo(path);

			if (fileInfo.Length == 0)
				result = CoreConstants.StorageMarker.Concat(result).ToArray();

			using (var stream = new FileStream(path, FileMode.Append))
				stream.Write(result, 0, result.Length);
		}

		internal static bool Exists(string path) =>
			!string.IsNullOrEmpty(path) && File.Exists(path);
    }
}
