using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace SqlUpdateManager.Core.Data
{
	internal class BsonSerializer
	{
		private readonly JsonSerializerSettings _settings;

		internal BsonSerializer()
		{
			_settings = new JsonSerializerSettings();

			_settings.Error += (object s, Newtonsoft.Json.Serialization.ErrorEventArgs a) =>
				throw new JsonSerializationException($"Error parsing JSON. {a.ErrorContext.Error.Message}");
			_settings.NullValueHandling = NullValueHandling.Include;
			_settings.ObjectCreationHandling = ObjectCreationHandling.Reuse;
			_settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			_settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
		}

		internal TType Deserialize<TType>(byte[] data)
		{
			using (var ms = new MemoryStream(data))
			using (var bs = new BsonDataReader(ms))
			{
				bs.ReadRootValueAsArray = true;
				var serializer = new JsonSerializer();
				return serializer.Deserialize<TType>(bs);
			}
		}

		internal JToken Deserialize(string data)
		{
			var bytes = Convert.FromBase64String(data);

			using (var ms = new MemoryStream(bytes))
			using (var bs = new BsonDataReader(ms))
			{
				bs.ReadRootValueAsArray = true;
				var serializer = new JsonSerializer();
				return serializer.Deserialize<JToken>(bs);
			}
		}

		internal byte[] Serialize(object data)
		{
			using (var ms = new MemoryStream())
			using (var bs = new BsonDataWriter(ms))
			{
				var serializer = new JsonSerializer();
				serializer.Serialize(bs, data);
				return ms.ToArray();
			}
		}
	}
}
