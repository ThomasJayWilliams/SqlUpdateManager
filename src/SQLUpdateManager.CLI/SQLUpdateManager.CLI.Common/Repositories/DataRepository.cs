using SqlUpdateManager.Core.Common;
using System;
using System.IO;

namespace SqlUpdateManager.CLI.Common
{
    public class DataRepository : IDataRepository
    {
        private readonly ISerializer _serializer;
        private readonly IFileManager _fileManager;

        public DataRepository(ISerializer serializer, IFileManager fileManager)
        {
            _serializer = serializer;
            _fileManager = fileManager;
        }

        public T GetData<T>(string path)
        {
            if (!_fileManager.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            var raw = _fileManager.Load(path);

            var data = _serializer.Deserialize<T>(raw);

            if (data == null)
                throw new NullReferenceException("Raw data is invalid or empty.");

            return data;
        }

        public string GetRawData(string path)
        {
            if (!_fileManager.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            var raw = _fileManager.Load(path);

            if (raw == null)
                throw new NullReferenceException("Raw data is invalid or empty.");

            return raw;
        }

        public void WriteData(string path, object data)
        {
            if (!_fileManager.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            var serialized = _serializer.Serialize(data);

            if (string.IsNullOrEmpty(serialized))
                throw new NullReferenceException("Error serializing passed object.");

            _fileManager.Save(path, serialized);
        }

        public void WriteRawData(string path, string data)
        {
            if (!_fileManager.Exists(path))
                throw new FileNotFoundException("Specified file could not be found.");

            _fileManager.Save(path, data);
        }

        public bool Exists(string path) =>
            _fileManager.Exists(path);
    }
}
