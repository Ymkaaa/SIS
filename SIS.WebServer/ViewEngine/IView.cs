namespace SIS.WebServer.ViewEngine
{
    public interface IView
    {
        string GetHtml(object Model);
    }
}
