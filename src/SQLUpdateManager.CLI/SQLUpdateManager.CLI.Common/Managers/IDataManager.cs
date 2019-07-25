namespace SQLUpdateManager.CLI.Common
{
    public interface IDataManager
    {
        TModel GetData<TModel>(string path);
        string GetRawData(string path);

        void WriteData(string path, object data);
        void WriteRawData(string path, string data);
    }
}
