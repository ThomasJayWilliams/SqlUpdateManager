using SQLUpdateManager.CLI.Application;

namespace SQLUpdateManager.CLI
{
    public interface IDataObjectsFactory
    {
        ICommand GetCommand(string name);
        IParameter GetParameter(string name);
    }
}
