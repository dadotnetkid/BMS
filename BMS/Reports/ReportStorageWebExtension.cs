using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.Web.Extensions;
using Helpers;
using Models.Repository;

namespace BMS.Reports
{
    public class ReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public override bool CanSetData(string url)
        {
            return unitOfWork.TemplatesRepo.Fetch(m => m.ReportName == url).Any();
        }
        public override bool IsValidUrl(string url)
        {
            
            return unitOfWork.TemplatesRepo.Fetch(m => m.ReportName == url).Any();
        }

        public override byte[] GetData(string url)
        {
            
            var model = unitOfWork.TemplatesRepo.Find(m => m.ReportName == url);

            return model?.Template;
        }

        public override Dictionary<string, string> GetUrls()
        {
          
            var templates = unitOfWork.TemplatesRepo.Get().ToDictionary(x => x.ReportName, x => x.ReportName);



            return templates;
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);

                var model = unitOfWork.TemplatesRepo.Find(m => m.ReportName == url);
                model.Template = ms.GetBuffer();
                unitOfWork.Save();
            }
          
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                report.SaveLayoutToXml(ms);
                if (!unitOfWork.TemplatesRepo.Fetch(m => m.ReportName == defaultUrl).Any())
                {
                    report.SaveLayoutToXml(ms);
                    unitOfWork.TemplatesRepo.Insert(new Models.Templates()
                    {
                        ReportName = defaultUrl,
                        Template = ms.ToArray()
                    });
                    unitOfWork.Save();
                }
                else
                {
                    report.SaveLayoutToXml(ms);

                    var model = unitOfWork.TemplatesRepo.Find(m => m.ReportName == defaultUrl);
                    model.Template = ms.GetBuffer();
                    unitOfWork.Save();
                }
            }
            return unitOfWork.TemplatesRepo.Find().ReportName.ToString();
        }
    }
}
