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
    }
}
