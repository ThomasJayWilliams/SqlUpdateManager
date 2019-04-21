namespace SUM.System.IO
{
	public interface IInputHandler
	{
		ActionDTO ReadCommandInput();
		InputGate ReadGateInput();
	}
}
