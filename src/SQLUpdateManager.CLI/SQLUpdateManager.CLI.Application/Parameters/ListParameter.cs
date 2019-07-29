using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.Application
{
    public class ListParameter : IParameter
    {
        public string Argument { get; set; }
        public bool RequiresArgument { get => false; }
        public string Name { get => CLIConstants.ListParameter; }
    }
}
