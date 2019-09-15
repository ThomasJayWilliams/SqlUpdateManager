using SqlUpdateManager.Core.Data;

namespace SqlUpdateManager.Core
{
	public static class Core
	{
		private static StorageContext _context;

		public static StorageContext GetContext(string path)
		{
			_context = _context ?? new StorageContext(path);

			return _context;
		}
	}
}
