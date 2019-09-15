using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SqlUpdateManager.Core.Registry
{
	public class Register
    {
        private readonly ISerializer _serializer;

        private List<Server> _servers = new List<Server>();

        public Register(ISerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException("Serializer cannot be null!");
        }

        public void AddServer(Server data)
        {
            _servers = Load()
                .ToList();

            if (data == null)
                throw new ArgumentNullException("Server cannot be null!");
			if (_servers.Select(server => server.Hash).Any(hash => hash.SequenceEqual(data.Hash)))
				throw new DuplicateException("Specified server already exists!");

            _servers.Add(data);
        }

        public void RemoveServer(byte[] hash)
        {
            _servers = Load()
                .ToList();

            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException("Server hash cannot be null or empty!");

            if (_servers.FirstOrDefault(server => server.Hash.SequenceEqual(hash)) == null)
                throw new ArgumentException("Server not found!");

            _servers.Remove(_servers.FirstOrDefault(server => server.Hash.SequenceEqual(hash)));
        }

        public void UpdateServer(Server data)
        {
            _servers = Load()
                .ToList();

            if (data == null)
                throw new ArgumentNullException("Server cannot be null!");

            var server = _servers.FirstOrDefault(serv => serv.Hash.SequenceEqual(data.Hash));

            if (server == null)
                throw new ArgumentException("Server does not exist!");

            server.Databases = data.Databases;
            server.Name = data.Name;
            server.Type = data.Type;
        }

        public IEnumerable<Server> GetAll() =>
            new ReadOnlyCollection<Server>(Load().ToList());

        public bool IsExist(byte[] serverHash)
        {
            _servers = Load()
                .ToList();

            return _servers.Any(s => s.Hash.SequenceEqual(serverHash));
        }

        public Server GetServer(byte[] hash)
        {
            _servers = Load()
                .ToList();

            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException("Server hash cannot be null or empty!");

            var server = _servers.FirstOrDefault(serv => serv.Hash.SequenceEqual(hash));

            return server;
        }

        public Server GetServerByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Server name cannot be null or empty.");

            _servers = Load()
                .ToList();

            return _servers.FirstOrDefault(s => s.Name == name);
        }

        public void SaveChanges() =>
            FileManager.Save(_serializer.Path, _serializer.Serialize(_servers));

        private IEnumerable<Server> Load()
        {
            if (!FileManager.Exists(_serializer.Path))
                return Enumerable.Empty<Server>();

			var str = FileManager.Load(_serializer.Path);

			if (string.IsNullOrEmpty(str))
				return Enumerable.Empty<Server>();

            var data = _serializer
                .Deserialize<IEnumerable<Server>>(str);

            return data;
        }
    }
}
