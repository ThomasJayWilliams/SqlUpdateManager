using SQLUpdateManager.Core.Common;
using SQLUpdateManager.Core.Domains;
using SQLUpdateManager.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLUpdateManager.Core.Registration
{
    public class Register
    {
        private readonly string _path;
        private readonly ISerializer _serializer;

        private List<Server> _servers;

        public Register(ISerializer serializer, string path)
        {
            _path = path ?? throw new ArgumentNullException("File path cannot be null or emnpty!");
            _serializer = serializer ?? throw new ArgumentNullException("Serializer canont be null!");

            _servers = new List<Server>();
            _servers = Load()
                .ToList();
        }

        public void AddServer(Server data)
        {
            if (data == null)
                throw new ArgumentNullException("Server cannot be null!");

            _servers.Add(data);
        }

        public void RemoveServer(string hash)
        {
            if (string.IsNullOrEmpty(hash))
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
            server.Users = data.Users;
        }

        public Server GetServer(string hash)
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentNullException("Server hash cannot be null or empty!");

            var server = _servers.FirstOrDefault(serv => serv.Hash == hash);

            if (server == null)
                throw new ArgumentException("Server does not exist!");

            return server;
        }

        public void SaveChanges()
        {
            foreach (var server in _servers)
            {
                foreach (var database in server.Databases)
                    foreach (var procedure in database.Procedures)
                        procedure.Data = Compressor.Compress(procedure.Data);

                foreach (var user in server.Users)
                    user.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));
            }

            FileManager.Save(_path, _serializer.Serialize(_servers));
        }

        private IEnumerable<Server> Load()
        {
            var data = _serializer
                .Deserializer<IEnumerable<Server>>(FileManager.Load(_path));

            foreach (var server in data)
            {
                foreach (var database in server.Databases)
                    foreach (var procedure in database.Procedures)
                        procedure.Data = Compressor.Decompress(procedure.Data);

                foreach (var user in server.Users)
                    user.Password = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));
            }

            return data;
        }
    }
}
