using SQLUpdateManager.Core.Internal;
using System.Collections.Generic;
using System;

namespace SQLUpdateManager.Core.Domains
{
    public class Database : IData
    {
        public string Name { get; set; }
        public byte[] Hash
        {
            get => Hasher.GetHash($"{Name}");
        }
        public IEnumerable<Procedure> Procedures { get; set; }

		public Database(params Procedure[] procedures)
		{
			Procedures = procedures ?? throw new ArgumentNullException("Cannot create database with no stored procedures!");
		}
    }
}
