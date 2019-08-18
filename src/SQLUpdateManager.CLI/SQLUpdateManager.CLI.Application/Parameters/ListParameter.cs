using SqlUpdateManager.CLI.Common;

namespace SqlUpdateManager.CLI.Application
{
    public class ListParameter : IParameter
    {
        public string Argument { get; set; }
        public string Name { get => CLIConstants.ListParameter; }
    }
}
