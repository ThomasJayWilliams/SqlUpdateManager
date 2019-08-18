using SqlUpdateManager.CLI.Application;

namespace SqlUpdateManager.CLI
{
    public interface ICommandParser
    {
        ICommand Parse(string input);
    }
}
