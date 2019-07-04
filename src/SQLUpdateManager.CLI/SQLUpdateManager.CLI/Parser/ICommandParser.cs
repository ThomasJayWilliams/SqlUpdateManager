using SQLUpdateManager.CLI.Application;

namespace SQLUpdateManager.CLI
{
    public interface ICommandParser
    {
        ICommand Parse(string input);
    }
}
