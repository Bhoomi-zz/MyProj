using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Commands;
using Website.CommandService;
using ReadModel;
using System.Linq;
using Website.Services;
using Website.ViewModel;
namespace Website.Controllers
{

    [HandleError, Authorize]
    public class SharedController : Controller
    {
        //private Services.SharedDataService _sharedDataService;

        public ActionResult FindContactPerson(string q, int limit, string contactId)
        {
           // _sharedDataService = new SharedDataService();
            var contactPersons = SharedDataService.GetContactPersons(q, contactId);
            
            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, contactPersons);
            string json = Encoding.Default.GetString(ms.ToArray());
            json = json.Replace(@"""Value"":", @"""value"":");

            ms.Close();
            var jsonSerializer = new JavaScriptSerializer();
            var jsonString = jsonSerializer.Serialize(contactPersons).ToString();
            return Content(json);
        }

        public ActionResult FindLocation(string q, int limit)
        {

            //string q="";
            //int limit;
            //SharedDataService = new SharedDataService();
            var locations = SharedDataService.GetLocationByName(q.Trim());

            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, locations);
            string json = Encoding.Default.GetString(ms.ToArray());
            json = json.Replace(@"""Value"":", @"""value"":");
            ms.Close();
            var jsonSerializer = new JavaScriptSerializer();
            var jsonString = jsonSerializer.Serialize(locations).ToString();
            //var json = "";
            //foreach(KeyValuePair<string,string > keyValuePair in locations)
            //{
            //    if (json.Length > 0)
            //        json += ";";
            //    json += keyValuePair.Key.Trim() + ":" + keyValuePair.Value.Trim();
            //}
            return Content(json);
        }
        public ActionResult FindLocationFilterdWithRegion(string q, int limit, string region)
        {

            var locations = SharedDataService.GetLocationByName(q.Trim(), region);

            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, locations);
            string json = Encoding.Default.GetString(ms.ToArray());
            json = json.Replace(@"""Value"":", @"""value"":");
            ms.Close();
            var jsonSerializer = new JavaScriptSerializer();
            var jsonString = jsonSerializer.Serialize(locations).ToString();
            
            return Content(json);
        }
        public ActionResult GetAllUsers()
        {
            string q = "";
            int limit;
            //_sharedDataService = new SharedDataService();
            var users = SharedDataService.GetAllUsersDictionary();
            var json = "";
            foreach (KeyValuePair<string, string> keyValuePair in users)
            {
                if (json.Length > 0)
                    json += ";";
                json += keyValuePair.Key.Trim() + ":" + keyValuePair.Value.Trim();
            }
            return Content(json);
        }

        public ActionResult FindVendor(string q, int limit)
        {
            var locations = SharedDataService.GetVendorByName(q);
            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, locations);
            string json = Encoding.Default.GetString(ms.ToArray());
            json = json.Replace(@"""Value"":", @"""value"":");
            ms.Close();
            return Content(json); 
        }
        public ActionResult GetRegionInfo(Guid PlanDetailsId, string region)
        {
            string jsonResult = "";
            jsonResult = SharedDataService.GetRegionInfoForPlanRegion(PlanDetailsId, region);
            return Content(jsonResult);
        }

        public ActionResult ValidateBriefAndGetClientInfoByBriefId(int briefNo, Guid planDetailsId)
        {
            string jsonResult = "";
            jsonResult = SharedDataService.CheckIfPlanAlreadyGenerated(briefNo.ToString(), planDetailsId);
            jsonResult += SharedDataService.GetClientInfoByBriefId(briefNo.ToString());
            return Content(jsonResult);
        }

        public ActionResult GetLocationInfo(Guid PlanDetailsId, string region, string locationId)
        {
            string jsonResult = "";
            jsonResult = SharedDataService.GetLocationInfoForPlan(PlanDetailsId, region, locationId);
            return Content(jsonResult);
        }
        public ActionResult FindSites(string q, int limit)
        {
            var Sites = SharedDataService.GetSiteBySiteNo(q);
            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, Sites);
            string json = Encoding.Default.GetString(ms.ToArray());
            json = json.Replace(@"""Value"":", @"""value"":");
            ms.Close();
            return Content(json); 
        }
        public ActionResult FindSitesByName(string q, int limit)
        {
            var Sites = SharedDataService.GetSiteBySiteName(q);
            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, Sites);
            string json = Encoding.Default.GetString(ms.ToArray());
            json = json.Replace(@"""Value"":", @"""value"":");
            ms.Close();
            return Content(json);
        }
    }
}