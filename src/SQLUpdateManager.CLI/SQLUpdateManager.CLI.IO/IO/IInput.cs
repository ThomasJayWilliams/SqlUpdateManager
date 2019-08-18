using System;

namespace SqlUpdateManager.CLI.IO
{
    public interface IInput
    {
        string ReadLine();
        ConsoleKeyInfo ReadKey();
        int ReadChar();
        string ReadPassword();
    }
}
