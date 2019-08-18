namespace SqlUpdateManager.CLI.Common
{
    public interface ICommonConfiguration
    {
        void ConfigureSession(AppConfig config, Storage storage);
    }
}
