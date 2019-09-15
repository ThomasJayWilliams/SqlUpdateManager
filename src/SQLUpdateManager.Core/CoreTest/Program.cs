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

			context.Servers.Value.Add(new ServerEntity
			{
				Address = "test",
				Name = "name",
				Password = "pass"
			});

			context.SaveChanges();

			foreach (var server in context.Servers.Value.AsNoTracking())
				Console.WriteLine($"{server.Name}, {server.Password}");
		}
	}
}
