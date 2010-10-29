using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Website.Services;
using Website.ViewModel;
using Website.ViewModel.Email;
using CrystalDecisions.CrystalReports.Engine;
using Ncqrs;
using System.Reflection;

namespace Website.Controllers
{
    [HandleError, Authorize]
    public class EmailController : Controller
    {
        //
        // GET: /Email/
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            IEnumerable<PlanIndexViewModel> viewModels = SharedDataService.GetAllPlans();
            return View(viewModels);
        }
        public ActionResult SendEmail(Guid id)
        {
            //var plan = SharedDataService.GetPlanById(id);
            //var regions = SharedDataService.GetAllRegionsForPlan(id);
            //var cities = SharedDataService.GetAllCitiesForPlanRegion(id, "<All>");
            //var sites = SharedDataService.GetAllSitesForPlan(id);
            try
            {
                EmailViewModel viewModel = new EmailViewModel();
                viewModel = SharedDataService.GetEmailViewModel(id);
                viewModel.EmailDataViewModels = SharedDataService.GetEmailDataViewModel(id);
                viewModel.PlanDetailsId = id;
                viewModel.FromAddress = new System.Configuration.AppSettingsReader().GetValue("FromAddressForEmail", typeof(string)).ToString();
                //viewModel.ToAddress = plan.EmailId;
                viewModel.Subject = "Laqshya Media Pvt Ltd, Plan Details for Plan No : " + viewModel.PlanNo;

                string fileName = new Random().Next().ToString() + "_PlanEmail.pdf";
                ReportClass rptH = new ReportClass();

                rptH.FileName = Request.PhysicalApplicationPath + "\\Reports\\PlanEmail.rpt";
                viewModel.FileName = fileName;
                rptH.Load();
                rptH.Database.Tables[0].SetDataSource(viewModel.EmailDataViewModels);
                viewModel.Content = BuildContentForEmail(viewModel.EmailDataViewModels);
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                FileStream fs = System.IO.File.Create(Request.PhysicalApplicationPath + "\\Reports\\" + fileName);

                using (Stream output = fs)
                {
                    byte[] buffer = new byte[32 * 1024];
                    int read;

                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, read);
                    }
                }
                return View(viewModel);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
            //return File(stream, "application/pdf");   
           
        }
        private string BuildContentForEmail(IEnumerable<EmailDataViewModel> siteInfo)
        {
            string val = "";
            val = "Dear Sir/Madam,";
            val += Environment.NewLine + Environment.NewLine;
            val += "Hereby I submit the plan details attached in this email.";
            val += Environment.NewLine;
            val += "Below is the summary of the Plan ";
            val += Environment.NewLine;
            var city = "";
            val += "Budget : " + siteInfo.Select(x => x.budget).FirstOrDefault() + Environment.NewLine;
            val += "Total  : " + siteInfo.Sum(x => x.Cost) + Environment.NewLine;
 
            foreach (var site in siteInfo)
            {
                if (city != site.sLocationName)
                {
                    val += Environment.NewLine + "City : " + site.sLocationName +", Total : " + siteInfo.Where(x1=>x1.sLocationName == city).Sum(x=>x.Cost) + Environment.NewLine ;
                    city = site.sLocationName;
                }
                val += "            Site : " + site.siteName + ", Rate : "+ site.Cost + Environment.NewLine; 
            }
            val += Environment.NewLine + "Warm Regards," + Environment.NewLine + User.Identity.Name + ".";
            return val;
        }

        public ActionResult SendEmailWithAttachment(Guid planDetailsId, string fromAddress, string toAddress, string subject, string content, string fileName )
        {
            try
            {
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = content,
                };
                message.Attachments.Add(new Attachment(Request.PhysicalApplicationPath + "\\Reports\\" + fileName));
                var client = new SmtpClient();
                client.EnableSsl = true;
                client.Send(message);
                return Content("Success");
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }
    }
}
