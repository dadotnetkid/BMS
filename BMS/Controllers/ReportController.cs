using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Helpers;
using Models.Repository;
using Models.ViewModels;

namespace BMS.Controllers
{
    [RoutePrefix("report")]
    public class ReportController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [HttpPost]
        public ActionResult ShowReportViewer(ParamerterViewModels item)
        {



            return PartialView(item);
        }

        [Route("certifications")]
        public ActionResult Certifications()
        {
            return View();
        }

        // GET: Report

        #region Certifications

        public ActionResult PrintCertificate(ParamerterViewModels item)
        {
            var model = unitOfWork.CertificationsRepo.Get(m => m.Id == item.Id);
            XtraReport xtraReport = new XtraReport();
            if (model.Any())
            {
                if (model.FirstOrDefault().CertificationTypes.CertificationType == "Barangay Clearance")
                {
                    xtraReport = new rptBarangayClearance() { DataSource = model };
                }
                else if (model.FirstOrDefault().CertificationTypes.CertificationType == "Resident Certification")
                {
                    xtraReport = new rptResidents() { DataSource = model };
                }
                else if (model.FirstOrDefault().CertificationTypes.CertificationType == "Business Permit")
                {
                    xtraReport = new rptBusinessCertificate() { DataSource = model };
                }
            }
            return PartialView(xtraReport);
        }

        [ValidateInput(false)]
        public ActionResult CertificateGridViewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Certifications item)
        {
            var model = unitOfWork.CertificationsRepo.Get();
            ViewData["model"] = item;

            if (item.PersonalInformationId > 0)
                if (unitOfWork.CertificationTypesRepo.Find(m => m.Id == item.CertificationTypeId).CertificationType == "Barangay Clearance" && unitOfWork.BlottersRepo.Fetch(m => m.ViolatorId == item.PersonalInformationId).Any(m => m.IsCleared != true))
                {
                    ModelState.AddModelError("PersonalInformationId", "Citizen has a previous blotter case that is not yet cleared");
                    ViewData["EditError"] = "Citizen has a previous blotter case that is not yet cleared";
                    ViewData["model"] = item;
                    return PartialView("_CertificateGridViewPartial", unitOfWork.CertificationsRepo.Get());
                }
            return PartialView("_CertificateGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "new certificate")]
        public ActionResult CertificateGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Certifications item)
        {

            if (item.PersonalInformationId > 0)
                if (unitOfWork.CertificationTypesRepo.Find(m => m.Id == item.CertificationTypeId).CertificationType == "Barangay Clearance" && unitOfWork.BlottersRepo.Fetch(m => m.ViolatorId == item.PersonalInformationId).Any(m => m.IsCleared != true))
                {
                    ModelState.AddModelError("PersonalInformationId", "Citizen has a previous blotter case that is not yet cleared");
                    ViewData["EditError"] = "Citizen has a previous blotter case that is not yet cleared";
                    ViewData["model"] = item;
                    return PartialView("_CertificateGridViewPartial", unitOfWork.CertificationsRepo.Get());
                }
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.CertificationsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
                ViewData["model"] = item;
            }

            var model = unitOfWork.CertificationsRepo.Get();
            return PartialView("_CertificateGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit certificate")]
        public ActionResult CertificateGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Certifications item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.CertificationsRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.CertificationsRepo.Get();
            return PartialView("_CertificateGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete certificate")]
        public ActionResult CertificateGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.CertificationsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.CertificationsRepo.Get();
            return PartialView("_CertificateGridViewPartial", model);
        }

        public ActionResult AddEditCertification([ModelBinder(typeof(DevExpressEditorsBinder))] int? certificateId, [ModelBinder(typeof(DevExpressEditorsBinder))] Models.Certifications item)
        {
            var model = unitOfWork.CertificationsRepo.Find(m => m.Id == certificateId);
            return PartialView(model);
        }


        #endregion
    }
}