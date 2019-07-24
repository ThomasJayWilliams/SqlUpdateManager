using Newtonsoft.Json;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Common;

namespace SQLUpdateManager.CLI
{
    public class JsonSerializer : ISerializer
    {
        public string Path { get => Constants.RegisterPath; }

        public T Deserialize<T>(string data) =>
            JsonConvert.DeserializeObject<T>(data);

        public string Serialize(object data) =>
            JsonConvert.SerializeObject(data);
    }
}
