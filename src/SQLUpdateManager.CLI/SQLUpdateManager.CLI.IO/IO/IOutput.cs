namespace SqlUpdateManager.CLI.IO
{
	public interface IOutput
    {
        void Print(string data);
        void PrintEmptyLine();
        void PrintLine(string data);
        void PrintASCII(string[] art);
    }
}
