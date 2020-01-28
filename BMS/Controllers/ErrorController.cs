using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Helpers;

namespace BMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        [Route("transaction-error")]
        public ActionResult TransactionsError([ModelBinder(typeof(DevExpressEditorsBinder))]
            string error)
        {
            ViewBag.Error = error;

            return PartialView();
        }
        [Route("access-denied/{actionName}")]
        public ActionResult AccessDenied(string returnUrl, string actionName)
        {
            var onUserAuthorization = new OnUserAuthorizationAttribute(actionName);
            if (onUserAuthorization.IsAuthenticated)
                return Redirect(returnUrl);
            return View();
        }
        [Route("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}