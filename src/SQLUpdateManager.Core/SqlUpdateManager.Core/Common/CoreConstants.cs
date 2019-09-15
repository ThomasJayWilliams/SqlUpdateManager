namespace SqlUpdateManager.Core
{
	public static class CoreConstants
	{
		public const string StorageExtension = ".storage";

		public static byte[] StorageMarker = new byte[8]
		{
			17, 235, 14, 42, 21, 0, 255, 6
		};

		public const string AppConfigExtension = ".cfg";

		public static byte[] AppConfigMarker = new byte[8]
		{
			12, 25, 51, 12, 62, 234, 9, 2
		};
	}
}
