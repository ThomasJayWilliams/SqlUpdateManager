using SQLUpdateManager.Core.Common;
using System;

namespace SQLUpdateManager.CLI.Common
{
    public class DataManager : IDataManager
    {
        private readonly IFileManager _fileManager;
        private readonly ISerializer _serializer;

        public DataManager(IFileManager fileManager, ISerializer serializer)
        {
            _fileManager = fileManager;
            _serializer = serializer;
        }

        public T GetData<T>(string path)
        {
            var raw = _fileManager.Read(path);

            if (string.IsNullOrEmpty(raw))
                throw new NullReferenceException("File is empty.");

            var data = _serializer.Deserialize<T>(raw);

            if (data == null)
                throw new NullReferenceException("Raw data is invalid or empty.");

            return data;
        }

        public string GetRawData(string path)
        {
            var raw = _fileManager.Read(path);

            if (string.IsNullOrEmpty(raw))
                throw new NullReferenceException("File is empty.");

            return raw;
        }

        public void WriteData(string path, object data)
        {
            var serialized = _serializer.Serialize(data);

            if (string.IsNullOrEmpty(serialized))
                throw new NullReferenceException("Error serializing passed object.");

            _fileManager.Write(path, serialized);
        }

        public void WriteRawData(string path, string data)
        {
            _fileManager.Write(path, data);
        }
    }
}
