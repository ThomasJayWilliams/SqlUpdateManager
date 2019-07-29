using SQLUpdateManager.CLI.Common;
using System;
using System.Drawing;

namespace SQLUpdateManager.CLI.IO
{
    public class Input : IInput
    {
        private readonly Session _session;

        public Input(Session session)
        {
            _session = session;
        }

        public string ReadLine()
        {
            Colorful.Console.ForegroundColor = Color.FromArgb(
                _session.Theme.InputColor.R,
                _session.Theme.InputColor.G,
                _session.Theme.InputColor.B);

            return Colorful.Console.ReadLine();
        }

        public ConsoleKeyInfo ReadKey() =>
            Colorful.Console.ReadKey();

        public int ReadChar() =>
            Colorful.Console.Read();

        public string ReadPassword()
        {
            var password = string.Empty;
            while (true)
            {
                var key = Colorful.Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    return password;
                if (key.Key == ConsoleKey.Backspace)
                    password = password.Substring(0, password.Length - 1);

                else
                    password += key.KeyChar;
            }
        }
    }
}
