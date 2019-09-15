using SqlUpdateManager.Core;
using SqlUpdateManager.Core.Data;
using System;
using System.IO;

namespace CoreTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var path = "test.storage";
			var context = Core.GetContext(path);

			if (!File.Exists(path))
				File.Create(path).Dispose();

			context.Servers.Add(new ServerEntity
			{
				Address = "test",
				Name = "name",
				Password = "pass"
			});

			context.Servers.SaveChanges();

			foreach (var server in context.Servers.AsNoTracking())
				Console.WriteLine($"{server.Name}, {server.Password}");

			Console.ReadLine();
		}
	}
}
