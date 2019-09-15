namespace SqlUpdateManager.Core
{
	public static class CoreConstants
	{
		public const string StorageExtension = ".storage";

		public static byte[] StorageMarker = new byte[8]
		{
			17, 235, 14, 42, 21, 0, 255, 6
		};
	}
}
