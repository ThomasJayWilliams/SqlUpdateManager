namespace SUM.Core.IO
{
	public interface IInputHandler
	{
		ActionDTO ReadCommandInput();
		InputGate ReadGateInput();
	}
}
