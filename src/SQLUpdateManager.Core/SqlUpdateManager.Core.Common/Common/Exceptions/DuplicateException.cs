using System;

namespace SqlUpdateManager.Core.Common
{
	public class DuplicateException : Exception
	{
		public DuplicateException(string message) : base(message) { }
	}
}
