using System;

namespace SQLUpdateManager.CLI.IO
{
    public static class InputHandler
    {
        public static string ReadLine() =>
            Console.ReadLine();

        public static ConsoleKeyInfo ReadKey() =>
            Console.ReadKey();

        public static int ReadChar() =>
            Console.Read();

        public static string ReadPassword()
        {
            var password = string.Empty;
            while (true)
            {
                var key = Console.ReadKey(true);

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
