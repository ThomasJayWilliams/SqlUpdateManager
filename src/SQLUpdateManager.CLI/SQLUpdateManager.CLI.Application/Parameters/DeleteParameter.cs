using SqlUpdateManager.CLI.Common;

namespace SqlUpdateManager.CLI.Application
{
    public class DeleteParameter : IParameter
    {
        public string Argument { get; set; }
        public string Name { get => CLIConstants.DeleteParameter; }
    }
}
