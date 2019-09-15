﻿using System.IO;
using System.IO.Compression;

namespace SqlUpdateManager.Core
{
	internal static class Compressor
    {
		internal static byte[] Compress(byte[] data)
		{
			var output = new MemoryStream();

			using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
				dstream.Write(data, 0, data.Length);

			return output.ToArray();
		}

		internal static byte[] Decompress(byte[] data)
		{
			var output = new MemoryStream();

			using (var dStream = new DeflateStream(new MemoryStream(data), CompressionMode.Decompress))
				dStream.CopyTo(output);

			return output.ToArray();
		}
	}
}
