using System.Web;

public static class HtmlHelperExtensions
{
    public static IHtmlString ShowError(this System.Web.Mvc.HtmlHelper helper, string error)
    {
        return new HtmlString("<script>alert ('" + error + "');</script>");
    }
}