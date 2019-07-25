using SQLUpdateManager.Core.Common;
using System;
using System.IO;

namespace SQLUpdateManager.CLI.Common
{
    public class DataRepository : IDataRepository
    {
        private readonly ISerializer _serializer;

        public DataRepository(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public T GetData<T>(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            using (var stream = new StreamReader(path))
            {
                var raw = stream.ReadToEnd();

                if (string.IsNullOrEmpty(raw))
                    throw new NullReferenceException("File is empty.");

                var data = _serializer.Deserialize<T>(raw);

                if (data == null)
                    throw new NullReferenceException("Raw data is invalid or empty.");

                return data;
            }
        }

        public string GetRawData(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            using (var stream = new StreamReader(path))
            {
                var raw = stream.ReadToEnd();

                if (string.IsNullOrEmpty(raw))
                    throw new NullReferenceException("File is empty.");

                return raw;
            }
        }

        public void WriteData(string path, object data)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            var serialized = _serializer.Serialize(data);

            if (string.IsNullOrEmpty(serialized))
                throw new NullReferenceException("Error serializing passed object.");

            using (var file = new StreamWriter(path))
            {
                var bytes = Session.Current.Encoding.GetBytes(serialized);
                file.Write(serialized);
            }
        }

        public void WriteRawData(string path, string data)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            using (var file = File.OpenWrite(path))
            {
                var bytes = Session.Current.Encoding.GetBytes(data);
                file.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
