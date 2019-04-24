namespace SUM.Core
{
	public interface IStartup
	{
		IConfigurator Configurator { get; set; }

		void RunApp();
	}
}
