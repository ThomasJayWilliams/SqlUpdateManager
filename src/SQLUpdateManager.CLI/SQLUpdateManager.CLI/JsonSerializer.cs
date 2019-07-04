using Newtonsoft.Json;
using SQLUpdateManager.Core.Common;

namespace SQLUpdateManager.CLI
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string data) =>
            JsonConvert.DeserializeObject<T>(data);

        public string Serialize(object data) =>
            JsonConvert.SerializeObject(data);
    }
}
