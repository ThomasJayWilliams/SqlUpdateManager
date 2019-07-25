using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.Application
{
    public class StateCommand : BaseCommand
    {
        public override string Name { get => Constants.StateCommand; }

        public override bool RequiresParameters { get => true; }
        public override bool RequiresArgument { get => false; }

        public override void Execute()
        {
            
        }
    }
}
