using System;

namespace SqlUpdateManager.Core
{
	public class DuplicateException : Exception
	{
		public DuplicateException(string message) : base(message) { }
	}
}
