using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.Application
{
    public class ListParameter : IParameter
    {
        public string Argument { get; set; }
        public string Name { get => CLIConstants.ListParameter; }
    }
}
