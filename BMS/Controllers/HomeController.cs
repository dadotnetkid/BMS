using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Repository;

namespace BMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PersonalInformationPartial()
        {
            var residents = unitOfWork.PersonalInformationsRepo.Fetch().Count();
            return PartialView(residents);
        }

        public ActionResult MeetingsPartial()
        {
            return PartialView(unitOfWork.MeetingsRepo.Fetch().Count());
        }
        public ActionResult ActivitiesPartial()
        {
            return PartialView(unitOfWork.ActivitiesRepo.Fetch().Count());
        }

        public ActionResult BlottersPartial()
        {
            return PartialView(unitOfWork.BlottersRepo.Fetch().Count());
        }
        [ValidateInput(false)]
        public ActionResult BlottersGridViewPartial()
        {
            var model = unitOfWork.BlottersRepo.Get();
            return PartialView("_BlottersGridViewPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult PersonalInformationGridViewPartial()
        {
            var model = unitOfWork.PersonalInformationsRepo.Get();
            return PartialView("_PersonalInformationGridViewPartial", model);
        }
        public ActionResult MeetingGridViewPartial()
        {
            var model = unitOfWork.MeetingsRepo.Get();
            return PartialView("_MeetingGridViewPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult ActivityGridViewPartial()
        {
            var model = unitOfWork.ActivitiesRepo.Get();
            return PartialView("_ActivityGridViewPartial", model);
        }
    }
}