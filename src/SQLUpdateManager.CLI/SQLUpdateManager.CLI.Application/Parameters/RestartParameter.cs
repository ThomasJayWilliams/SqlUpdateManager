using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.Application
{
    public class RestartParameter : IParameter
    {
        public string Argument { get; set; }
        public bool RequiresArgument { get => false; }
        public string Name { get => CLIConstants.RestartParameter; }
    }
}
