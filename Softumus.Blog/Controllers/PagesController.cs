using System.Web.Mvc;
using Softumus.Blog.Models;

namespace Softumus.Blog.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Pages/

        public ActionResult Index(string date, string name)
        {
            var model = Page.GetByDate(date);

            return View(model);
        }

        public ActionResult All()
        {
            var model = new AllPagesModel
            {
                PageDescriptions = Page.GetAll()
            };

            return View(model);
        }

        public ActionResult Latest()
        {
            return View("Index", Page.GetLatest());
        }
    }
}
