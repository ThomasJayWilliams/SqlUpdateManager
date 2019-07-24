using SQLUpdateManager.Core.Common;
using SQLUpdateManager.Core.Domains;
using SQLUpdateManager.Core.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLUpdateManager.Core.Registration
{
	public class Register
    {
        private readonly ISerializer _serializer;

        private List<Server> _servers;

        public Register(ISerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException("Serializer cannot be null!");
            
            _servers = Load()
                .ToList();
        }

        public void AddServer(Server data)
        {
            if (data == null)
                throw new ArgumentNullException("Server cannot be null!");
			if (_servers.Select(server => server.Hash).Any(hash => hash == data.Hash))
				throw new DuplicateException("Specified server already exists!");

            _servers.Add(data);
        }

        public void RemoveServer(byte[] hash)
        {
            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException("Server hash cannot be null or empty!");

            if (_servers.FirstOrDefault(server => server.Hash == hash) == null)
                throw new ArgumentException("Server not found!");

            _servers.Remove(_servers.FirstOrDefault(server => server.Hash == hash));
        }

        public void UpdateServer(Server data)
        {
            if (data == null)
                throw new ArgumentNullException("Server cannot be null!");

            var server = _servers.FirstOrDefault(serv => serv.Hash == data.Hash);

            if (server == null)
                throw new ArgumentException("Server does not exist!");

            server.Databases = data.Databases;
            server.Name = data.Name;
            server.Type = data.Type;
        }

        public Server GetServer(byte[] hash)
        {
            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException("Server hash cannot be null or empty!");

            var server = _servers.FirstOrDefault(serv => serv.Hash == hash);

            if (server == null)
                throw new ArgumentException("Server does not exist!");

            return server;
        }

        public void SaveChanges() =>
            FileManager.Save(_serializer.Path, _serializer.Serialize(_servers));

        private IEnumerable<Server> Load()
        {
            if (!File.Exists(_serializer.Path))
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
