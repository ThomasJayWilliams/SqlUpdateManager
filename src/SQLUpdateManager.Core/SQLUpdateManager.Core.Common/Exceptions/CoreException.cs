using System;

namespace SQLUpdateManager.Core.Common
{
	public class CoreException : Exception
	{
		public CoreErrorCodes Code { get; set; }

		public CoreException(string message) : base(message) { }

		public CoreException(string message, CoreErrorCodes code) : base(message)
		{
			Code = code;
		}
	}
}
