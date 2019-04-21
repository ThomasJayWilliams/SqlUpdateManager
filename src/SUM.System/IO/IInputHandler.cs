namespace SUM.System.IO
{
	public interface IInputHandler
	{
		InputCommand ReadCommandInput();
		InputGate ReadGateInput();
	}
}
