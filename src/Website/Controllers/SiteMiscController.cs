using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CommonDTOs;
using Ncqrs;
using Website.CommandService;
using Website.Services;
using Website.ViewModel.Plan;

namespace Website.Controllers
{
   [HandleError,  Authorize]
    public class SiteMiscController : Controller
    {
        private const string SessionKeyCurrentPage = "SiteMiscController_CurrentPage";
        private const string SessionKeySites = "SiteMiscController_Sites";
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<PlanSite> Sites
        {
            get
            {
                if (Session[SessionKeySites] != null)
                    return (List<PlanSite>)Session[SessionKeySites];
                else
                    return new List<PlanSite>();
            }
            set
            {
                Session[SessionKeySites] = value;
            }
        }

        public ActionResult IndexForVendorInfoChange()
        {
            var items = SharedDataService.GetAllPlans();
            return View(items);
        }

        public ActionResult SiteVendorAssignment(Guid planDetailsId)
        {
            SiteVendorAssignmentViewModel viewModel = SharedDataService.GetPlanSiteVendorAssignmentByPlanId(planDetailsId);
            Sites = null;
            viewModel.Regions = SharedDataService.GetAllRegionsForPlan(planDetailsId);
            viewModel.Cities = SharedDataService.GetAllCitiesForPlanRegion(planDetailsId, "<All>");
            return View(viewModel);
        }

        public ActionResult GetSites(Guid planDetailsId, string region, string CityId, string activity)
        {
            var pageIndex = 1;
            var pageSize = 1;
            var totalRecords = 1;
            var totalPages = 1;
            
            if (Sites.Count == 0)
            {
                Sites = SharedDataService.GetAllSitesForPlan(planDetailsId).ToList();
            }
            var obj = (from x in Sites.AsEnumerable().Where(x => (x.Region == region || region == "" || region=="<All>") && (x.CityId == CityId || CityId == ""))
                       select new
                       {
                           i = x.PlanSiteId,
                           cell = new string[]
                                                 {
                                                     x.IsSelected,
                                                     "",
                                                     x.PlanSiteId.ToString(),
                                                      x.Region,
                                                     x.CityName,
                                                     x.MediaType,
                                                     x.SiteName, 
                                                     x.Illumination,
                                                     x.H.ToString(),
                                                     x.V.ToString(), 
                                                     x.SizeRatio,
                                                     x.Days.ToString(),
                                                     x.Qty.ToString(),
                                                     x.SiteSizeInSqFt.ToString(),
                                                     activity=="Display" ? x.DisplayVendor  : (activity=="Mounting"? x.MountingVendor : activity=="Fabrication" ? x.FabricationVendor : (activity=="Printing"? x.PrintingVendor: x.DisplayVendor )), 
                                                     activity=="Display" ? x.DisplayVendorName  : (activity=="Mounting"? x.MountingVendorName : activity=="Fabrication" ? x.FabricationVendorName : (activity=="Printing"? x.PrintingVendorName: x.DisplayVendorName )),
                                                     activity=="Display" ? x.DisplayRate.ToString() : (activity=="Mounting"? x.MountingRate.ToString() : activity=="Fabrication" ? x.FabricationRate.ToString(): (activity=="Printing"? x.PrintingRate.ToString()    : x.DisplayRate.ToString() )),
                                                     activity=="Display" ? x.DisplayCost.ToString() : (activity=="Mounting"? x.MountingCost.ToString() : activity=="Fabrication" ? x.FabricationCost.ToString() : (activity=="Printing"? x.PrintingCost.ToString(): x.DisplayCost.ToString() )),
                                                     activity=="Display" ? x.DisplayClientRate.ToString() : (activity=="Mounting"? x.MountingClientRate.ToString() : activity=="Fabrication" ? x.FabricationClientRate.ToString() : (activity=="Printing"? x.PrintingClientRate.ToString(): x.DisplayClientRate.ToString() )),
                                                     activity=="Display" ? x.DisplayClientCost.ToString() : (activity=="Mounting"? x.MountingClientCost.ToString() : activity=="Fabrication" ? x.FabricationClientCost.ToString() : (activity=="Printing"? x.PrintingClientCost.ToString(): x.DisplayClientCost.ToString() )),
                                                     x.StartDate.ToString() != "" ? x.StartDate.Value.ToShortDateString() : x.StartDate.ToString(),
                                                     x.EndDate.ToString() != "" ? x.EndDate.Value.ToShortDateString() : x.EndDate.ToString() ,
                                                     activity=="Display" ? x.DisplayStatus : (activity=="Mounting"? x.MountingStatus : activity=="Fabrication" ? x.FabricationStatus : (activity=="Printing"? x.PrintingStatus: x.DisplayStatus ))
                                                 }
                       });
            var jsonData = new
            {
                total = totalPages,
                page = totalPages,
                records = totalRecords,
                rows = obj.ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalculateCosts(string activity)
        {
            int totalDisplayCost = Convert.ToInt32( Sites.Sum(x => x.DisplayCost));

            int totalMountingCost = Convert.ToInt32( Sites.Sum(x => x.MountingCost));
            int totalPrintingCost = Convert.ToInt32(Sites.Sum(x => x.PrintingCost));
            int totalFabricationCost = Convert.ToInt32(Sites.Sum(x => x.FabricationCost));

            int totalDisplayClientCost = Convert.ToInt32(Sites.Sum(x => x.DisplayClientCost));
            int totalMountingClientCost = Convert.ToInt32(Sites.Sum(x => x.MountingClientCost));
            int totalPrintingClientCost = Convert.ToInt32(Sites.Sum(x => x.PrintingClientCost));
            int totalFabricationClientCost = Convert.ToInt32(Sites.Sum(x => x.FabricationClientCost));

            int displayBookedCount = Sites.Where(x => x.DisplayStatus == "Booked").Count();
            int displayPendingCount = Sites.Where(x => x.DisplayStatus == "" || x.DisplayStatus ==null).Count();
            int displayProposedCount = Sites.Where(x => x.DisplayStatus == "Proposed").Count();
            int displayUNCount = Sites.Where(x => x.DisplayStatus == "Under Negotiation").Count();

            int mountingBookedCount = Sites.Where(x => x.MountingStatus == "Booked").Count();
            int mountingPendingCount = Sites.Where(x => x.MountingStatus == "" ||x.MountingStatus ==null ).Count();
            int mountingProposedCount = Sites.Where(x => x.MountingStatus == "Proposed").Count();
            int mountingUNCount = Sites.Where(x => x.MountingStatus == "Under Negotiation").Count();

            int printingBookedCount = Sites.Where(x => x.PrintingStatus == "Booked" ).Count();
            int printingPendingCount = Sites.Where(x => x.PrintingStatus == "" || x.PrintingStatus ==null).Count();
            int printingProposedCount = Sites.Where(x => x.PrintingStatus == "Proposed").Count();
            int printingUNCount = Sites.Where(x => x.PrintingStatus == "Under Negotiation").Count();

            int fabricationBookedCount = Sites.Where(x => x.FabricationStatus == "Booked" ).Count();
            int fabricationPendingCount = Sites.Where(x => x.FabricationStatus == "" || x.FabricationStatus == null).Count();
            int fabricationProposedCount = Sites.Where(x => x.FabricationStatus == "Proposed").Count();
            int fabricationUNCount = Sites.Where(x => x.FabricationStatus == "Under Negotiation").Count();

            int totalSites = Sites.Count;
            return Content(totalDisplayCost + ";" + totalMountingCost + ";" + totalPrintingCost +";" + totalFabricationCost + ";" 
                + displayPendingCount + ";" + displayBookedCount + ";" + displayProposedCount + ";" + displayUNCount +";"
                + mountingPendingCount + ";" + mountingBookedCount + ";" + mountingProposedCount + ";" + mountingUNCount + ";" 
                + printingPendingCount + ";" + printingBookedCount + ";" + printingProposedCount + ";" + printingUNCount +";" 
                + fabricationPendingCount + ";" + fabricationBookedCount + ";" + fabricationProposedCount + ";" + fabricationUNCount + ";"
                + totalDisplayClientCost + ";" + totalMountingClientCost + ";" + totalPrintingClientCost + ";" + totalFabricationClientCost + ";" + totalSites);
        }

        [HttpPost]
        public ActionResult UpdateVendorInfo(FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            var planSite = new PlanSite();
            if (operation != null && operation.Equals("add"))
            {

                if (operation.Equals("add"))
                {
                    planSite = new PlanSite()
                                   {
                                       PlanSiteId = Guid.NewGuid()
                                   };
                    Sites.Add(planSite);
                }
            }
            else
            {
                planSite =
                    Sites.Where(
                        x =>
                        x.PlanSiteId == Guid.Parse(formCollection["PlanSiteId"])).
                        FirstOrDefault();
            }
            if (planSite != null)
            {
                planSite.SiteName = formCollection["SiteName"];
                planSite.Region = formCollection["Region"];
                planSite.CityId = formCollection["CityId"];
                planSite.CityName = formCollection["CityName"];
                planSite.ChargingBasis = formCollection["ChargingBasis"];

                string activity = formCollection["Activity"];
                if (!planSite.Activity.Contains(activity))
                    planSite.Activity += "," + activity;
                planSite.IsDirty = true;
                switch (activity)
                {
                    case "Display":
                        planSite.DisplayVendor = formCollection["selectedLocationVendor"] ??
                                                 formCollection["DisplayVendor"];
                        planSite.DisplayVendorName = formCollection["DisplayVendorName"];
                        planSite.DisplayCost = formCollection["DisplayCost"] == null
                                                   ? 0
                                                   : Convert.ToInt32(formCollection["DisplayRate"])*
                                                     Convert.ToInt32(formCollection["SiteSizeInSqFt"]);
                        planSite.DisplayRate = formCollection["DisplayRate"] == null
                                                   ? 0
                                                   : Convert.ToDecimal(formCollection["DisplayRate"]);
                        planSite.DisplayStatus = formCollection["Status"];

                        break;
                    case "Printing":
                        planSite.PrintingVendor = formCollection["selectedLocationVendor"] ??
                                                  formCollection["PrintingVendor"];
                        planSite.DisplayVendorName = formCollection["PrintingVendorName"];
                        planSite.PrintingCost = formCollection["PrintingCost"] == null
                                                    ? 0
                                                    : Convert.ToDecimal(formCollection["PrintingRate"])*
                                                      Convert.ToDecimal(formCollection["SiteSizeInSqFt"]);
                        planSite.PrintingRate = formCollection["PrintingRate"] == null
                                                    ? 0
                                                    : Convert.ToDecimal(formCollection["PrintingRate"]);
                        planSite.PrintingStatus = formCollection["Status"];
                        break;
                    case "Mounting":
                        planSite.MountingVendor = formCollection["selectedLocationVendor"] ??
                                                  formCollection["MountingVendor"];
                        planSite.MountingVendorName = formCollection["MountingVendorName"];
                        planSite.MountingCost = formCollection["MountingCost"] == null
                                                    ? 0
                                                    : Convert.ToDecimal(formCollection["MountingRate"])*
                                                      Convert.ToDecimal(formCollection["SiteSizeInSqFt"]);
                        planSite.MountingRate = formCollection["MountingRate"] == null
                                                    ? 0
                                                    : Convert.ToDecimal(formCollection["MountingRate"]);
                        planSite.MountingStatus = formCollection["Status"];
                        break;
                    case "Fabrication":
                        planSite.FabricationVendor = formCollection["selectedLocationVendor"] ??
                                                     formCollection["FabricationVendor"];
                        planSite.FabricationVendorName = formCollection["FabricationVendorName"];
                        planSite.FabricationCost = formCollection["FabricationCost"] == null
                                                       ? 0
                                                       : Convert.ToDecimal(formCollection["FabricationRate"])*
                                                         Convert.ToDecimal(formCollection["SiteSizeInSqFt"]);
                        planSite.FabricationRate = formCollection["FabricationRate"] == null
                                                       ? 0
                                                       : Convert.ToDecimal(formCollection["FabricationRate"]);
                        planSite.FabricationStatus = formCollection["Status"];
                        break;
                }
                if (formCollection["EndDate"] != null)
                    planSite.EndDate = Convert.ToDateTime(formCollection["EndDate"]);

                planSite.H = formCollection["H"] == null ? 0 : Convert.ToInt32(formCollection["H"]);
                planSite.Illumination = formCollection["Illumination"];
                planSite.MediaType = formCollection["MediaType"];
                planSite.SiteName = formCollection["SiteName"];
                planSite.SizeRatio = formCollection["SizeRatio"];
                if (formCollection["StartDate"] != null)
                    planSite.StartDate = Convert.ToDateTime(formCollection["StartDate"]);
                planSite.V = formCollection["V"] == null ? 0 : Convert.ToInt32(formCollection["V"]);
            }

            return Content("Success");
        }

       public ActionResult SelectAllSites(Guid planDetailsId, string region, string CityId, string activity)
        {
            foreach (var planSite in Sites)
            {
                planSite.IsSelected = "true";
            }
            return GetSites(planDetailsId, region, CityId, activity);
        }

        public ActionResult SelectNone(Guid planDetailsId, string region, string CityId, string activity)
        {
            foreach (var planSite in Sites)
            {
                planSite.IsSelected = "false";
            }
            return GetSites(planDetailsId, region, CityId, activity);
        }

        public ActionResult SelectSpecificSite(Guid siteId, string isSelected)
        {
            PlanSite site = Sites.Where(x => x.PlanSiteId == siteId).FirstOrDefault();
            if (site != null)
            {
                site.IsSelected = isSelected;
                return Content("Success");
            }
            else
            {
                return Content("Failure");
            }
        }

        [HttpPost]
        public ActionResult UpdateSiteInfoOnEdit(FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            var planSite = new PlanSite();
            if (operation != null && operation.Equals("add"))
            {

                if (operation.Equals("add"))
                {
                    planSite = new PlanSite()
                    {
                        PlanSiteId = Guid.NewGuid()
                    };
                    Sites.Add(planSite);
                }
            }
            else
            {
                planSite =
                    Sites.Where(
                        x =>
                        x.PlanSiteId == Guid.Parse(formCollection["PlanSiteId"])).
                        FirstOrDefault();
            }
            if (planSite != null)
            {
                string activity = formCollection["Activity"];
                if(activity==null)
                    return Content("Failure");
                string vendorid;
                if(formCollection["selectedVendor"]!=null)
                    vendorid= formCollection["selectedVendor"].ToString();
                else
                {
                    vendorid = formCollection["VendorId"].ToString();
                }
                string vendorName = formCollection["VendorName"];
                decimal rate = formCollection["Rate"] == null  ? 0 : Convert.ToDecimal( formCollection["Rate"]);
                decimal siteSizeInSqFt = formCollection["SiteSizeInSqFt"] == null ? 0 : Convert.ToDecimal(formCollection["SiteSizeInSqFt"]);
                decimal clientRate =formCollection["ClientRate"] == null  ? 0 : Convert.ToDecimal( formCollection["ClientRate"]);
                //decimal cost = siteSizeInSqFt*rate;
                decimal cost = formCollection["Cost"] == null ? 0 : Convert.ToDecimal(formCollection["Cost"]);
                
                decimal clientCost = formCollection["clientCost"] == null ? 0 : Convert.ToDecimal(formCollection["clientCost"]);
                if (!planSite.Activity.Contains(activity))
                    planSite.Activity += "," + activity;
                planSite.IsDirty = true;
                planSite.ChargingBasis = formCollection["ChargingBasis"];
                planSite.IsSelected = formCollection["IsSelected"];
                switch (activity)
                {
                    case "Display":
                        planSite.DisplayVendor = vendorid;
                        planSite.DisplayVendorName = vendorName;
                        planSite.DisplayCost =Convert.ToInt32(  planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : cost);
                        planSite.DisplayRate = rate;
                        planSite.DisplayStatus = formCollection["Status"];
                        planSite.DisplayClientRate = clientRate;
                        planSite.DisplayClientCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : clientCost;
                        break;
                    case "Printing":
                        planSite.PrintingVendor = vendorid;
                        planSite.PrintingVendorName = vendorName;
                        planSite.PrintingCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : cost;
                        planSite.PrintingRate = rate;
                        planSite.PrintingStatus = formCollection["Status"];
                        planSite.PrintingClientRate = clientRate;
                        planSite.PrintingClientCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : clientCost;
                        break;
                    case "Mounting":
                        planSite.MountingVendor = vendorid;
                        planSite.MountingVendorName = vendorName;
                        planSite.MountingCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : cost;
                        planSite.MountingRate = rate;
                        planSite.MountingStatus = formCollection["Status"];
                        planSite.MountingClientRate = clientRate;
                        planSite.MountingClientCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : clientCost;
                        break;
                    case "Fabrication":
                        planSite.FabricationVendor = vendorid;
                        planSite.FabricationVendorName = vendorName;
                        planSite.FabricationCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : cost;
                        planSite.FabricationRate = rate;
                        planSite.FabricationStatus = formCollection["Status"];
                        planSite.FabricationClientRate = clientRate;
                        planSite.FabricationClientCost = planSite.ChargingBasis == "Per SqFt" ? rate * siteSizeInSqFt : clientCost;
                        break;
                }

                if (formCollection["EndDate"] != null)
                    planSite.EndDate = Convert.ToDateTime(formCollection["EndDate"]);
                
                if (formCollection["StartDate"] != null)
                    planSite.StartDate = Convert.ToDateTime(formCollection["StartDate"]);
                
            }
            return Content("Success");
        }

        public ActionResult AssignVendorInfo(Guid planDetailsId, string activity, string chargeBasis, string status, string region, string cityId, string VendorName, string VendorId, decimal Rate, string fromDate, string toDate, decimal clientRate, string chargingBasis)
        {
            DateTime dtFromdate, dtToDate;
            decimal cost = 0, calcRate= 0, calcClientRate=0;
            decimal clientCost = 0;
            calcRate = Rate;
            calcClientRate = clientRate;
            IEnumerable<PlanSite> planSites =
                Sites.Where(
                    x =>
                    x.IsSelected == "true" && (region == "" || x.Region == region) &&
                    (cityId == "" || x.CityId == cityId));

            foreach (var planSite in planSites)
            {
                //if (planSite.IsSelected != null && planSite.IsSelected.Equals("true") && (region == "" || planSite.Region == region) && (cityId=="" || planSite.CityId == cityId))
                //{
                     switch (chargeBasis)
                     {
                         case "Per SqFt":
                             cost = Rate*planSite.SiteSizeInSqFt;
                             clientCost = clientRate*planSite.SiteSizeInSqFt;
                             break;
                         case "Per Site" :
                             cost = Rate;
                             clientCost = clientRate;
                             break;
                         case "Lump sum":
                             calcRate = Rate / planSites.Count();
                             cost = calcRate;
                             calcClientRate = clientRate / planSites.Count();
                             clientCost = calcClientRate;
                             break;
                         default:
                             cost = 0;
                             clientCost = 0;
                             break;
                     }
                     if (!planSite.Activity.Contains(activity))
                         planSite.Activity += "," + activity;
                     planSite.IsDirty = true;
                    planSite.ChargingBasis = chargingBasis;
                    switch (activity)
                    {
                        case "Display":
                            planSite.DisplayVendor = VendorId !=""? VendorId : planSite.DisplayVendor;
                            planSite.DisplayVendorName = VendorName != ""? VendorName : planSite.DisplayVendorName;
                            planSite.DisplayRate = Rate != 0 ?  calcRate : planSite.DisplayRate;

                            planSite.DisplayCost = Convert.ToInt32( cost !=0 ? cost: planSite.DisplayCost);
                            if (fromDate != "")
                            {
                                DateTime.TryParse(fromDate, out dtFromdate);
                                planSite.StartDate = dtFromdate;
                            }
                            if (toDate != "")
                            {
                                DateTime.TryParse(toDate, out dtToDate);
                                planSite.EndDate = dtToDate;
                            }
                            planSite.DisplayStatus = status != "" ? status : planSite.DisplayStatus;
                            planSite.DisplayClientRate = clientRate != 0 ? calcClientRate : planSite.DisplayClientRate;
                            planSite.DisplayClientCost = clientCost != 0 ? clientCost : planSite.DisplayClientCost;
                            break;
                        case "Mounting":
                            planSite.MountingVendor = VendorId != "" ? VendorId : planSite.MountingVendor;
                            planSite.MountingVendorName = VendorName != "" ? VendorName : planSite.MountingVendorName;
                            planSite.MountingRate = Rate != 0 ? calcRate : planSite.MountingRate;
                            planSite.MountingCost = cost !=0 ? cost: planSite.MountingCost;
                            planSite.MountingStatus = status != "" ? status : planSite.MountingStatus;
                            planSite.MountingClientRate = clientRate != 0 ? calcClientRate : planSite.MountingClientRate;
                            planSite.MountingClientCost = clientCost != 0 ? clientCost : planSite.MountingClientCost;
                            break;
                        case "Printing" :
                            planSite.PrintingVendor =  VendorId != "" ? VendorId : planSite.PrintingVendor;
                            planSite.PrintingVendorName =  VendorName != "" ? VendorName : planSite.PrintingVendorName;
                            planSite.PrintingRate =  Rate != 0 ? calcRate : planSite.PrintingRate;
                            planSite.PrintingCost = cost != 0 ? cost : planSite.PrintingCost;
                            planSite.PrintingStatus = status != "" ? status : planSite.PrintingStatus;
                            planSite.PrintingClientRate = clientRate != 0 ? calcClientRate : planSite.PrintingClientRate;
                            planSite.PrintingClientCost = clientCost != 0 ? clientCost : planSite.PrintingClientCost;
                            break;
                        case "Fabrication":
                            planSite.FabricationVendor = VendorId != "" ? VendorId : planSite.FabricationVendor;
                            planSite.FabricationVendorName =  VendorName != "" ? VendorName : planSite.FabricationVendorName;
                            planSite.FabricationRate = Rate != 0 ? calcRate : planSite.FabricationRate;
                            planSite.FabricationCost = cost != 0 ? cost : planSite.FabricationCost;
                            planSite.FabricationStatus = status != "" ? status : planSite.FabricationStatus;
                            planSite.FabricationClientRate = clientRate != 0 ? calcClientRate : planSite.FabricationClientRate;
                            planSite.FabricationClientCost = clientCost != 0 ? clientCost : planSite.FabricationClientCost;
                            break;
                    }
                //}
            }
            return GetSites(planDetailsId, region, cityId, activity);
        }
        
        /* not needed as submit is done thru javascript
        [HttpPost]
        public ActionResult SiteVendorAssignment(SiteVendorAssignmentViewModel viewModel)
        {
            var command = new Commands.AssignVendorsToSite();
            command.PlanDetailsId = viewModel.PlanDetailsId;
            command.Sites = new List<SiteVendorAssignmentDTO>();
            foreach (var site in Sites)
            {
                if (site.IsDirty)
                {
                    CommonDTOs.SiteVendorAssignmentDTO assignmentDto = new SiteVendorAssignmentDTO();
                    var types = site.Activity.Split(',');
                     
                    foreach (var type in types)
                    {
                        if (type.Length == 0)
                            continue;

                        assignmentDto.PlanCityId = site.PlanCityId;
                        assignmentDto.PlanSiteId = site.PlanSiteId;
                        assignmentDto.VendorType = type;
                        assignmentDto.ChargingBasis = site.ChargingBasis;
                        switch (type)
                        {
                            case "Display":
                                assignmentDto.Vendor = site.DisplayVendor;
                                assignmentDto.Rate = site.DisplayRate;
                                assignmentDto.Cost = site.DisplayCost;
                                assignmentDto.StartDate = site.StartDate;
                                assignmentDto.EndDate = site.EndDate;
                                assignmentDto.Status = site.DisplayStatus;
                                assignmentDto.ClientRate = site.DisplayClientRate;
                                assignmentDto.ClientCost = site.DisplayClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                            case "Mounting":
                                assignmentDto.Vendor = site.MountingVendor;
                                assignmentDto.Rate = site.MountingRate;
                                assignmentDto.Cost = site.MountingCost;
                                assignmentDto.Status = site.MountingStatus;
                                assignmentDto.ClientRate = site.MountingClientRate;
                                assignmentDto.ClientCost = site.MountingClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                            case "Fabrication":
                                assignmentDto.Vendor = site.FabricationVendor;
                                assignmentDto.Rate = site.FabricationRate;
                                assignmentDto.Cost = site.FabricationCost;
                                assignmentDto.Status = site.FabricationStatus;
                                assignmentDto.ClientRate = site.FabricationClientRate;
                                assignmentDto.ClientCost = site.FabricationClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                            case "Printing":
                                assignmentDto.Vendor = site.PrintingVendor;
                                assignmentDto.Rate = site.PrintingRate;
                                assignmentDto.Cost = site.PrintingCost;
                                assignmentDto.Status = site.PrintingStatus;
                                assignmentDto.ClientRate = site.PrintingClientRate;
                                assignmentDto.ClientCost = site.PrintingClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                        }
                    }
                }
            }
            if (command.Sites.Count > 0)
            {
                var service = new MyNotesCommandServiceClient();
                service.AssignVendorToSitesInfo(command);
            }
            string activity = viewModel.Activity;
            viewModel = SharedDataService.GetPlanSiteVendorAssignmentByPlanId(viewModel.PlanDetailsId);
            viewModel.Activity = activity;
            Sites = null;
            viewModel.Regions = SharedDataService.GetAllRegionsForPlan(viewModel.PlanDetailsId);
            viewModel.Cities = SharedDataService.GetAllCitiesForPlanRegion(viewModel.PlanDetailsId, "<All>");
            return View(viewModel);
        }
        */


        public ActionResult SaveVendorInfo(Guid planDetailsId)
        {
            try
            {
                var command = new Commands.AssignVendorsToSite();
                command.PlanDetailsId = planDetailsId;
                command.Sites = new List<SiteVendorAssignmentDTO>();
                foreach (var site in Sites.Where(x => x.IsDirty))
                {
                    
                    var types = site.Activity.Split(',');

                    foreach (var type in types)
                    {
                        if (type.Length == 0)
                            continue;
                        CommonDTOs.SiteVendorAssignmentDTO assignmentDto = new SiteVendorAssignmentDTO();
                        assignmentDto.PlanCityId = site.PlanCityId;
                        assignmentDto.PlanSiteId = site.PlanSiteId;
                        assignmentDto.VendorType = type;
                        assignmentDto.ChargingBasis = site.ChargingBasis;
                        switch (type)
                        {
                            case "Display":
                                assignmentDto.Vendor = site.DisplayVendor;
                                assignmentDto.Rate = site.DisplayRate;
                                assignmentDto.Cost = site.DisplayCost;
                                assignmentDto.StartDate = site.StartDate;
                                assignmentDto.EndDate = site.EndDate;
                                assignmentDto.Status = site.DisplayStatus;
                                assignmentDto.ClientRate = site.DisplayClientRate;
                                assignmentDto.ClientCost = site.DisplayClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                            case "Mounting":
                                assignmentDto.Vendor = site.MountingVendor;
                                assignmentDto.Rate = site.MountingRate;
                                assignmentDto.Cost = site.MountingCost;
                                assignmentDto.Status = site.MountingStatus;
                                assignmentDto.ClientRate = site.MountingClientRate;
                                assignmentDto.ClientCost = site.MountingClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                            case "Fabrication":
                                assignmentDto.Vendor = site.FabricationVendor;
                                assignmentDto.Rate = site.FabricationRate;
                                assignmentDto.Cost = site.FabricationCost;
                                assignmentDto.Status = site.FabricationStatus;
                                assignmentDto.ClientRate = site.FabricationClientRate;
                                assignmentDto.ClientCost = site.FabricationClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                            case "Printing":
                                assignmentDto.Vendor = site.PrintingVendor;
                                assignmentDto.Rate = site.PrintingRate;
                                assignmentDto.Cost = site.PrintingCost;
                                assignmentDto.Status = site.PrintingStatus;
                                assignmentDto.ClientRate = site.PrintingClientRate;
                                assignmentDto.ClientCost = site.PrintingClientCost;
                                command.Sites.Add(assignmentDto);
                                break;
                        }
                    }

                }
                if (command.Sites.Count > 0)
                {
                    var service = new MyNotesCommandServiceClient();
                    service.AssignVendorToSitesInfo(command);
                }
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
            return Content("Success");
        }
    }
}
