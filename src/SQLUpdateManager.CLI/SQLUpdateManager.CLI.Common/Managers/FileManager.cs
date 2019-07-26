﻿using SQLUpdateManager.Core.Internal;
using System;
using System.IO;

namespace SQLUpdateManager.CLI.Common
{
    public class FileManager : IFileManager
    {
        public bool Exists(string path) =>
            File.Exists(path);

        public string Load(string path)
        {
            using (var stream = new StreamReader(path))
            {
                var raw = stream.ReadToEnd();
                return raw;
            }
        }

        public void Save(string path, string content)
        {
            using (var file = new StreamWriter(path))
            {
                var bytes = Session.Current.Encoding.GetBytes(content);
                file.Write(content);
            }
        }
    }
}