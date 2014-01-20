using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Softumus.MarkdownBlog.App_Start
{
	public class HandleNotFoundAttribute : ActionFilterAttribute, IExceptionFilter
	{
	    public void OnException(ExceptionContext filterContext)
	    {
	        var httpException = filterContext.Exception.GetBaseException() as HttpException;
	        if (httpException != null && httpException.GetHttpCode() == (int)HttpStatusCode.NotFound)
	        {
	            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true; // Prevents IIS from intercepting the error and displaying its own content.
	            filterContext.ExceptionHandled = true;
	            filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
				filterContext.Result = new ViewResult 
				{
					ViewName = "_404",
					ViewData = filterContext.Controller.ViewData,
					TempData = filterContext.Controller.TempData
				};
	        }
	    }
	}
}