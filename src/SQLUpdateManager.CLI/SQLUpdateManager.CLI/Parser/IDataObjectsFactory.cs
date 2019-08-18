using SqlUpdateManager.CLI.Application;

namespace SqlUpdateManager.CLI
{
    public interface IDataObjectsFactory
    {
        ICommand GetCommand(string name);
        IParameter GetParameter(string name);
    }
}
