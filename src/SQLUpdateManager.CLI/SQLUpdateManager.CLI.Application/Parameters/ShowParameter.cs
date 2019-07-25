using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.Application
{
    public class ShowParameter : IParameter
    {
        public string Argument { get; set; }
        public bool RequiresArgument { get => false; }
        public string Name { get => Constants.ShowParameter; }
    }
}
