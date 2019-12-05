namespace SIS.WebServer.ViewEngine
{
    public interface IViewEngine
    {
        string Execute<T>(string viewContent, T model);
    }
}
