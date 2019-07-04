namespace SQLUpdateManager.CLI.Application
{
    public class SaveParameter : IParameter
    {
        public IArgument Argument { get; set; }
        public bool HasArgument { get => false; }
        public string Name { get => "save"; }
        public string Single { get => $"-{Name}"; }
        public string Double { get => $"--{Name}"; }
    }
}
