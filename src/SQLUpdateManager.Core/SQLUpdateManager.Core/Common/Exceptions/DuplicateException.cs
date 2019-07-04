using System;

namespace SQLUpdateManager.Core.Common
{
	public class DuplicateException : Exception
	{
		public DuplicateException(string message) : base(message) { }
	}
}
