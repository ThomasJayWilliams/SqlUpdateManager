using SqlUpdateManager.Core;
using SqlUpdateManager.Core.Data;
using System;
using System.IO;
using System.Linq;

namespace CoreTest
{
	public class Program
	{
		private static Random random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static void Main(string[] args)
		{
			var path = "test.storage";
			var context = Core.GetContext(path);

			if (!File.Exists(path))
				File.Create(path).Dispose();

			for (int i = 0; i < 1000; i++)
			{
				context.Servers.Add(new ServerEntity
				{
					Address = RandomString(5),
					Name = RandomString(5),
					Password = RandomString(5)
				});
			}

			context.Servers.SaveChanges();
		}
	}
}
