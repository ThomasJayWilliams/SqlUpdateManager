using SqlUpdateManager.CLI.Common;
using System;

namespace SqlUpdateManager.CLI.IO
{
    public class Input : IInput
    {
        private readonly Session _session;

        public Input(Session session)
        {
            _session = session;
        }

		public string ReadLine() =>
			Console.ReadLine();

        public ConsoleKeyInfo ReadKey() =>
            Console.ReadKey();

        public int ReadChar() =>
            Console.Read();

        public string ReadPassword()
        {
            var password = string.Empty;

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;
                if (key.Key == ConsoleKey.Backspace)
                    password = password.Substring(0, password.Length - 1);

                else
                    password += key.KeyChar;
            }

            Console.WriteLine();
            return password;
        }
    }
}
