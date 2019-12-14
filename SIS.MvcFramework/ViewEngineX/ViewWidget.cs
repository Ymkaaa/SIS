using System.IO;

namespace SIS.MvcFramework.ViewEngineX
{
    public abstract class ViewWidget : IViewWidget
    {
        private const string WidgetFolderPath = "Views/Shared/Validation/";
        private const string WidgetExtension = "vwhtml";

        public ViewWidget()
        {
        }

        public string Render()
        {
            return File.ReadAllText($"{WidgetFolderPath}{this.GetType().Name}.{WidgetExtension}");
        }
    }
}
