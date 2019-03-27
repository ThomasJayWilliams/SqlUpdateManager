namespace SUM.System
{
	public interface IStartup
	{
		IConfigurator Configurator { get; set; }

		void RunApp();
	}
}
