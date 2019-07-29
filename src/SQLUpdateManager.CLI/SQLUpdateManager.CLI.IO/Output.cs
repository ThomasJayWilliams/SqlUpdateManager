using System;

namespace SQLUpdateManager.CLI.IO
{
    public static class Output
    {
        public static void Print(string data) =>
            Console.Write(data);

        public static void PrintLine(string data) =>
            Console.WriteLine(data);

        public static void PrintColored(string data, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(data);
            Console.ForegroundColor = defaultColor;
        }

        public static void PrintEmptyLine() =>
            Console.WriteLine("");

        public static void PrintColoredLine(string data, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ForegroundColor = defaultColor;
        }

        public static void PrintError(string data)
        {

        }

        public static void PrintError(Exception ex, string message)
        {

        }

        public static void PrintError(Exception ex)
        {

        }

        public static void PrintInfo(string message)
        {

        }
    }
}
