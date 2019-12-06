namespace SIS.MvcFramework.ViewEngineX
{
    public interface IViewEngine
    {
        string Execute<T>(string viewContent, T model);
    }
}
