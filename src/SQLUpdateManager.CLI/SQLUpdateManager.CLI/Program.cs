﻿using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var app = new Startup();
			app.ConfigureServices();
			app.Configure(new AppConfig());
			app.RunApp();
		}
	}
}
