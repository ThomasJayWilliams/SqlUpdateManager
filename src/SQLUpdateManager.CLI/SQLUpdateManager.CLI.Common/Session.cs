using SQLUpdateManager.Core.Domains;
using System;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Common
{
	public class Session
	{
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public static Session Current { get; } = new Session();

        private Session() { }

		public T GetValue<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("Key cannot be null or empty!");

            if (!_values.TryGetValue(key, out var result))
                return default(T);

			T temp;

			try
			{
				temp = (T)result;
			}
			catch(Exception ex)
			{
				throw new InvalidCastException("Invalid value type!", ex);
			}

			return temp;
		}

		public void SetValue(string key, object data)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("Key cannot be null or empty!");
			if (data == null)
				throw new ArgumentNullException("Value cannot be null!");

            if (_values.ContainsKey(key))
                _values.Remove(key);

			_values.Add(key, data);
		}

        public DateTime ApplicationStartTime
        {
            get => GetValue<DateTime>("ApplicationStart");
            set => SetValue("ApplicationStart", value);
        }

        public int SessionLifeTime
        {
            get => GetValue<int>("SessionLifeTime");
            set => SetValue("SessionLifeTime", value);
        }

        public DataServer ConnectedServer
        {
            get => GetValue<DataServer>("ConnectedServer");
            set => SetValue("ConnectedServer", value);
        }

        public Database UsedDatabase
        {
            get => GetValue<Database>("UsedDatabase");
            set => SetValue("UsedDatabase", value);
        }
	}
}
