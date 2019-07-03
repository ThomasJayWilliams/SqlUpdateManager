namespace SQLUpdateManager.Core.Common
{
	public class DuplicateException : CoreException
	{
		public DuplicateException(string message) : base(message) { }

		public DuplicateException(string message, CoreErrorCodes code) : base(message, code) { }
	}
}
