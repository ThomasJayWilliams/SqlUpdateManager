namespace SqlUpdateManager.CLI
{
    public interface IMiddleware
    {
        void Invoke(RequestContext context);
    }
}
