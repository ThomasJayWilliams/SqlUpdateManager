using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace SqlUpdateManager.Core.Data
{
	internal class JsonSerializer
	{
		private readonly JsonSerializerSettings _settings;

		internal JsonSerializer()
		{
			_settings = new JsonSerializerSettings();

			_settings.Error += (object s, ErrorEventArgs a) =>
				throw new JsonSerializationException($"Error parsing JSON. {a.ErrorContext.Error.Message}");
			_settings.NullValueHandling = NullValueHandling.Include;
			_settings.ObjectCreationHandling = ObjectCreationHandling.Reuse;
			_settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			_settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
		}

		internal TType Deserialize<TType>(string data) =>
			JsonConvert.DeserializeObject<TType>(data);

		internal JToken Deserialize(string data) =>
			JsonConvert.DeserializeObject<JToken>(data);

		internal string Serialize(object data) =>
			JsonConvert.SerializeObject(data);
	}
}
