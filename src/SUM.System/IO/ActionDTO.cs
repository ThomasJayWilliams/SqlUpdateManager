using System.Collections.Generic;

namespace SUM.System.IO
{
	public class ActionDTO
	{
		public InputAction Action { get; set; }
		public IEnumerable<InputParameters> Parameters { get; set; }
		public IEnumerable<string> Arguments { get; set; }
	}
}
