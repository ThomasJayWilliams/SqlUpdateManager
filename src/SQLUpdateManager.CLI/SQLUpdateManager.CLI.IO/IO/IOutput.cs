using SQLUpdateManager.CLI.Common;
using System.Drawing;

namespace SQLUpdateManager.CLI.IO
{
    public interface IOutput
    {
        void PrintColored(string data, Color color);
        void PrintColored(string data, RGB color);
        void PrintEmptyLine();
        void PrintColoredLine(string data, Color color);
        void PrintColoredLine(string data, RGB color);
        void PrintASCII(string[] art, RGB color);
    }
}
