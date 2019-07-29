using SQLUpdateManager.CLI.Common;
using System;
using System.Drawing;
using static Colorful.Console;

namespace SQLUpdateManager.CLI.IO
{
    public static class Output
    {
        public static void Print(string data) =>
            Write(data);

        public static void PrintLine(string data) =>
            WriteLine(data);

        public static void PrintColored(string data, Color color) =>
            Write(data, color);

        public static void PrintColored(string data, RGB color) =>
            Write(data, Color.FromArgb(color.R, color.G, color.B));

        public static void PrintEmptyLine() =>
            WriteLine();

        public static void PrintColoredLine(string data, Color color) =>
            WriteLine(data, color);

        public static void PrintColoredLine(string data, RGB color) =>
            WriteLine(data, Color.FromArgb(color.R, color.G, color.B));

        public static void PrintASCII(string[] art, RGB color)
        {
            if (art.Length > byte.MaxValue)
                throw new ArgumentException("Provided ASCII art is too big to print.");

            var colorCopy = new RGB(color.R, color.G, color.B);

            checked
            {
                var rStep = (byte)(((colorCopy.R / 1.1) / art.Length));
                var gStep = (byte)(((colorCopy.G / 1.1) / art.Length));
                var bStep = (byte)(((colorCopy.B / 1.1) / art.Length));

                foreach (var line in art)
                {
                    WriteLine(line, Color.FromArgb(colorCopy.R, colorCopy.G, colorCopy.B));

                    colorCopy.R -= rStep;
                    colorCopy.G -= gStep;
                    colorCopy.B += bStep;
                }
            }

            WriteLine();
        }
    }
}
