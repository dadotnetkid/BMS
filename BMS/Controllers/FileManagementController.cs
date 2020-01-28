using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using Models.Repository;
using Models.ViewModels;
using Services;

namespace BMS.Controllers
{
    [RoutePrefix("file-management")]
    public class FileManagementController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: FileManagement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Templates()
        {
            return View();
        }
        [Route("blotters")]
        [OnUserAuthorization(ActionName = "blotters")]
        public ActionResult Blotters()
        {
            return View();
        }
        [Route("personal-information")]
        [OnUserAuthorization(ActionName = "personal-information")]
        public ActionResult PersonalInformation()
        {
            return View();
        }
        [Route("complaint-types")]
        [OnUserAuthorization(ActionName = "complaint-types")]
        public ActionResult ComplaintTypes()
        {
            return View();
        }
        [Route("meeting")]
        [OnUserAuthorization(ActionName = "meeting")]
        public ActionResult Meeting()
        {
            return View();
        }
        [Route("activity")]
        [OnUserAuthorization(ActionName = "Activity")]
        public ActionResult Activity()
        {
            return View();
        }
        [Route("certifications")]
        [OnUserAuthorization(ActionName = "certifications")]
        public ActionResult Certifications()
        {
            return View();
        }

        #region Blotters


        [ValidateInput(false)]
        public ActionResult BlottersGridViewPartial()
        {
            var model = unitOfWork.BlottersRepo.Get();
            return PartialView("_BlottersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "new blotter")]
        public ActionResult BlottersGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Blotters item)
        {
            item.BlotterId = new IdHelpers().BlotterId;
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.BlottersRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var model = unitOfWork.BlottersRepo.Get();
            return PartialView("_BlottersGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit blotter")]
        public ActionResult BlottersGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Blotters item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.BlottersRepo.TrackModifiedEntities(m => m.Id == item.Id, item);
                    unitOfWork.BlottersRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.BlottersRepo.Get();
            return PartialView("_BlottersGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete blotter")]
        public ActionResult BlottersGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.BlottersRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.BlottersRepo.Get();
            return PartialView("_BlottersGridViewPartial", model);
        }

        public ActionResult AddEditBlotterPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? blotterId, [ModelBinder(typeof(DevExpressEditorsBinder))]EditingMode editingMode)
        {
            var model = unitOfWork.BlottersRepo.Find(m => m.Id == blotterId);
            ViewBag.EditingMode = editingMode;
            return PartialView(model);
        }

        #endregion

        #region Personal Information

        public ActionResult UploadImagePartial()
        {
            return BinaryImageEditExtension.GetCallbackResult();
        }

        [ValidateInput(false)]
        public ActionResult PersonalInformationGridViewPartial()
        {
            var model = unitOfWork.PersonalInformationsRepo.Get();
            return PartialView("_PersonalInformationGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "new personal information")]
        public ActionResult PersonalInformationGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.PersonalInformations item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Groups = new List<Groups>();
                    foreach (var i in item.GroupIds)
                    {
                        item.Groups.Add(unitOfWork.GroupsRepo.Find(m => m.Id == i));
                    }
                    unitOfWork.PersonalInformationsRepo.Insert(item);
                    unitOfWork.Save();
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.PersonalInformationsRepo.Get();
            return PartialView("_PersonalInformationGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit personal information")]
        public ActionResult PersonalInformationGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.PersonalInformations item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.PersonalInformationsRepo.TrackModifiedEntities(m => m.Id == item.Id, item);


                    unitOfWork.PersonalInformationsRepo.Update(item);
                    unitOfWork.Save();
                    var p = unitOfWork.PersonalInformationsRepo.Find(m => m.Id == item.Id);
                    p.Groups.Clear();
                    foreach (var i in item.GroupIds)
                    {
                        p.Groups.Add(unitOfWork.GroupsRepo.Find(m => m.Id == i));
                    }

                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.PersonalInformationsRepo.Get();
            return PartialView("_PersonalInformationGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete personal information")]
        public ActionResult PersonalInformationGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.PersonalInformationsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.PersonalInformationsRepo.Get();
            return PartialView("_PersonalInformationGridViewPartial", model);
        }

        public ActionResult AddEditPersonalInformationPartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            int? personalInformationId)
        {
            var model = unitOfWork.PersonalInformationsRepo.Find(m => m.Id == personalInformationId);
            if (model != null)
            {
                model.GroupIds = model.Groups.Select(x => x.Id).ToList();
            }
            return PartialView(model);
        }


        #endregion

        #region Meeting



        #endregion

        #region ComplaintTypes

        [ValidateInput(false)]

        public ActionResult ComplaintTypeGridViewPartial()
        {
            var model = unitOfWork.ComplaintTypesRepo.Get();
            return PartialView("_ComplaintTypeGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "new complaint types")]
        public ActionResult ComplaintTypeGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.ComplaintTypes item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.ComplaintTypesRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ComplaintTypesRepo.Get();
            return PartialView("_ComplaintTypeGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit complaint types")]
        public ActionResult ComplaintTypeGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.ComplaintTypes item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.ComplaintTypesRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ComplaintTypesRepo.Get();
            return PartialView("_ComplaintTypeGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete complaint types")]
        public ActionResult ComplaintTypeGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.ComplaintTypesRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.ComplaintTypesRepo.Get();
            return PartialView("_ComplaintTypeGridViewPartial", model);
        }

        public ActionResult AddEditComplaintTypePartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            int? complaintTypeId)
        {
            var model = unitOfWork.ComplaintTypesRepo.Find(m => m.Id == complaintTypeId);
            return PartialView(model);
        }


        #endregion

        #region Meeting


        public HttpStatusCodeResult SendMeeting([ModelBinder(typeof(DevExpressEditorsBinder))]int? meetingId)
        {
            var meetings = unitOfWork.MeetingsRepo.Find(m => m.Id == meetingId);
            if (meetings == null)
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, "No Meeting Id Found");
            SmsServices smsServices = new SmsServices();
            if (meetings.Groups?.PersonalInformations != null)
                foreach (var i in meetings.Groups?.PersonalInformations)
                {
                    if (i.ContactNumber != null)
                        smsServices.Send(i.ContactNumber,
                            "Subject:" + meetings?.Subject + Environment.NewLine + "Date:" + meetings?.ActivityDate +
                            Environment.NewLine +
                            "Description:" + meetings?.Description);
                }

            meetings.IsSend = true;
            unitOfWork.Save();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public HttpStatusCodeResult SendActivity([ModelBinder(typeof(DevExpressEditorsBinder))]int? activityId)
        {
            var activities = unitOfWork.ActivitiesRepo.Find(m => m.Id == activityId);
            if (activities == null)
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, "No Activity Id Found");
            SmsServices smsServices = new SmsServices();
            if (activities.Groups?.PersonalInformations != null)
                foreach (var i in activities.Groups?.PersonalInformations)
                {
                    if (i.ContactNumber != null)
                        smsServices.Send(i.ContactNumber,
                            "Subject:" + activities?.Subject + Environment.NewLine + "Date:" +
                            activities?.ActivityDate + Environment.NewLine +
                            "Description:" + activities?.Description);
                }

            activities.IsSend = true;
            unitOfWork.Save();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [ValidateInput(false)]
        public ActionResult MeetingGridViewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] SearchViewModels searchViewModels)
        {
            var model = unitOfWork.MeetingsRepo.Get();
            return PartialView("_MeetingGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "new meeting")]
        public ActionResult MeetingGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Meetings item)
        {
            item.MeetingId = new IdHelpers().MeetingId;

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model.
                    unitOfWork.MeetingsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.MeetingsRepo.Get();
            return PartialView("_MeetingGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit meeting")]
        public ActionResult MeetingGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Meetings item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.MeetingsRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.MeetingsRepo.Get();
            return PartialView("_MeetingGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete meeting")]
        public ActionResult MeetingGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.MeetingsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.MeetingsRepo.Get();
            return PartialView("_MeetingGridViewPartial", model);
        }

        public ActionResult AddEditMeetingPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? meetingId)
        {
            var model = unitOfWork.MeetingsRepo.Find(m => m.Id == meetingId);
            return PartialView(model);
        }


        #endregion

        #region Activity


        [ValidateInput(false)]
        public ActionResult ActivityGridViewPartial()
        {
            var model = unitOfWork.ActivitiesRepo.Get();
            return PartialView("_ActivityGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "new activity")]
        public ActionResult ActivityGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Activities item)
        {
            item.ActivityId = new IdHelpers().ActivityId;
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.ActivitiesRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ActivitiesRepo.Get();
            return PartialView("_ActivityGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit activity")]
        public ActionResult ActivityGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Activities item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.ActivitiesRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ActivitiesRepo.Get();
            return PartialView("_ActivityGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete activity")]
        public ActionResult ActivityGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.ActivitiesRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.ActivitiesRepo.Get();
            return PartialView("_ActivityGridViewPartial", model);
        }

        public ActionResult AddEditActivityPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? activityId)
        {
            var model = unitOfWork.ActivitiesRepo.Find(m => m.Id == activityId);
            return PartialView(model);
        }
        #endregion

        #region Groups
        [Route("groups")]
        [OnUserAuthorization(ActionName = "groups")]
        public ActionResult Groups()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GroupsGridViewPartial()
        {
            var model = unitOfWork.GroupsRepo.Get();
            return PartialView("_GroupsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "add groups")]
        public ActionResult GroupsGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Groups item)
        {
            item.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model

                    unitOfWork.GroupsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.GroupsRepo.Get();
            return PartialView("_GroupsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "edit groups")]
        public ActionResult GroupsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Groups item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.GroupsRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.GroupsRepo.Get();
            return PartialView("_GroupsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "delete groups")]
        public ActionResult GroupsGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] System.String Id)
        {

            if (Id != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.GroupsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            var model = unitOfWork.GroupsRepo.Get();
            return PartialView("_GroupsGridViewPartial", model);
        }

        #endregion

        #region Templates

        [ValidateInput(false)]
        public ActionResult TemplateGridViewPartial()
        {
            var model = unitOfWork.TemplatesRepo.Get();
            return PartialView("_TemplateGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TemplateGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Templates item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.TemplatesRepo.Insert(item);
                    unitOfWork.Save();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.TemplatesRepo.Get();
            return PartialView("_TemplateGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TemplateGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Templates item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.TemplatesRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.TemplatesRepo.Get();
            return PartialView("_TemplateGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TemplateGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.TemplatesRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.TemplatesRepo.Get();
            return PartialView("_TemplateGridViewPartial", model);
        }


        public ActionResult AddEditTemplatePartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? templateId)
        {
            var model = unitOfWork.TemplatesRepo.Find(m => m.Id == templateId);
            return PartialView(model);
        }

        public ActionResult TemplateRichEditPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? templateId)
        {

            var model = unitOfWork.TemplatesRepo.Find(m => m.Id == templateId);
            return PartialView("_TemplateRichEditPartial", model);
        }
        public ActionResult TemplateReport()
        {
            var template = unitOfWork.TemplatesRepo.Find();
            //if (template != null)
            //    template.Template = template?.Template?.Replace("[FullName]", "mark Christopher");
            List<Templates> model = new List<Templates>() {
                template
            };
            rptResidents rptResidents = new rptResidents() { DataSource = model };
            return PartialView(rptResidents);
        }

        public ActionResult TemplateHtmlEditorPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? templateId)
        {
            var model = unitOfWork.TemplatesRepo.Find(m => m.Id == templateId);
            return PartialView("_TemplateHtmlEditorPartial", model);
        }
        public ActionResult TemplateHtmlEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("TemplateHtmlEditor", FileManagementControllerTemplateHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult TemplateHtmlEditorPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("TemplateHtmlEditor", FileManagementControllerTemplateHtmlEditorSettings.ImageUploadValidationSettings, FileManagementControllerTemplateHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }


        #endregion


        #region Cedulas

        public ActionResult AddEditCedulasPartial([ModelBinder(typeof(DevExpressEditorsBinder))] int? cedulaId)
        {

            return PartialView(unitOfWork.CedulasRepo.Find(m => m.Id == cedulaId));
        }
        public ActionResult Cedulas()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult CedulasGridViewPartial()
        {
            var model = unitOfWork.CedulasRepo.Get(includeProperties: "PersonalInformations");
            return PartialView("_CedulasGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CedulasGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Cedulas item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.CedulasRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.CedulasRepo.Get(includeProperties: "PersonalInformations");
            return PartialView("_CedulasGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CedulasGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Cedulas item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.CedulasRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.CedulasRepo.Get(includeProperties: "PersonalInformations");
            return PartialView("_CedulasGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CedulasGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] System.Int32 Id)
        {

            if (Id >= 0)
            {
                try
                {
                    unitOfWork.CedulasRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.CedulasRepo.Get(includeProperties: "PersonalInformations");
            return PartialView("_CedulasGridViewPartial", model);
        }


        #endregion


    }
    public class FileManagementControllerTemplateHtmlEditorSettings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "FileManagement", Action = "TemplateHtmlEditorPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

}