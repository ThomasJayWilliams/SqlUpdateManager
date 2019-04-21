using SUM.System;
using SUM.System.IO;

namespace SUM.Gates
{
	public class BinaryGate : IBinaryGate
	{
		private readonly IInputHandler iHandler;
		private readonly IOutputHandler oHandler;

		public BinaryGate(IInputHandler iHandler, IOutputHandler oHandler)
		{
			this.iHandler = iHandler;
			this.oHandler = oHandler;
		}

		public bool Request(string output)
		{
			oHandler.Out(output);

			if (iHandler.ReadGateInput() == InputGate.Accept)
				return true;

			return false;
		}
	}
}
