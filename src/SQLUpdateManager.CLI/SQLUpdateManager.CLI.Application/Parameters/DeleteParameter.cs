using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.Application
{
    public class DeleteParameter : IParameter
    {
        public string Argument { get; set; }
        public bool RequiresArgument { get => true; }
        public string Name { get => CLIConstants.DeleteParameter; }
    }
}
