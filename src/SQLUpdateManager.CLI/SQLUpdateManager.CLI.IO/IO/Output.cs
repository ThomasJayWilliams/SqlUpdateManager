using System;
using static System.Console;

namespace SQLUpdateManager.CLI.IO
{
	public class Output : IOutput
    {
        public void Print(string data) =>
            Write(data);

        public void PrintEmptyLine() =>
            WriteLine();

        public void PrintLine(string data) =>
            WriteLine(data);

        public void PrintASCII(string[] art)
        {
            if (art.Length > byte.MaxValue)
                throw new ArgumentException("Provided ASCII art is too big to print.");

                foreach (var line in art)
                    WriteLine(line);

            WriteLine();
        }
    }
}
