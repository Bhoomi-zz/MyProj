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
using Commands;

namespace Website.Controllers
{

    [HandleError, Authorize]
    public class SiteController : Controller
    {
        private const string SessionKeyCurrentPage = "SiteController_CurrentPage";
        private const string SessionKeyCurrentSite = "SiteController_CurrentSite";
        private const string SessionKeyDeselectedSite = "SiteController_DeselectedSite";
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<PlanSite> Sites
        {
            get
            {
                if (Session[SessionKeyCurrentSite] != null)
                    return (List<PlanSite>)Session[SessionKeyCurrentSite];
                else
                    return new List<PlanSite>();
            }
            set
            {
                Session[SessionKeyCurrentSite] = value;
            }
        }
        private List<PlanSite> DeselectedSites
        {
            get
            {

                if (Session[SessionKeyDeselectedSite] != null)
                    return (List<PlanSite>)Session[SessionKeyDeselectedSite];
                else
                    return new List<PlanSite>();
            }
            set
            {
                Session[SessionKeyDeselectedSite] = value;
            }
        }
        public ActionResult PlanSite(Guid planDetailId)
        {
             var planSiteViewModel = new PlanSiteViewModel();
            Sites = null;
            DeselectedSites = null;
            planSiteViewModel = SharedDataService.GetPlanSiteViewModelByPlanId(planDetailId);
            planSiteViewModel.Regions = SharedDataService.GetAllRegionsForPlan(planDetailId);
            planSiteViewModel.PlanCities = SharedDataService.GetAllCitiesForPlanRegion(planDetailId, "<All>");
            return View(planSiteViewModel);
        }
        //Callef from outside to add a new site.
        public ActionResult AddNewSite(Guid planDetailsId, string region, string locationId)
        {
            PlanSiteViewModel planSiteViewModel = SharedDataService.GetPlanSiteViewModelByPlanId(planDetailsId);
            planSiteViewModel.Regions = SharedDataService.GetAllRegionsForPlan(planDetailsId);
            planSiteViewModel.Region = region;
            planSiteViewModel.PlanCities = SharedDataService.GetAllCitiesForPlanRegion(planDetailsId, "<All>");
            planSiteViewModel.LocationId = locationId;
            planSiteViewModel.LocationName = locationId;
            return View("PlanSite", planSiteViewModel);
        }

        public ActionResult GetSites(Guid planDetailsId, string region, string locationId)
        {
            var pageIndex = 1;
            var pageSize = 1;
            var totalRecords = 1;
            var totalPages = 1;

            if (Sites.Count == 0 || Sites.Where(x => x.Region == region).Count() == 0)
            {
                Sites= SharedDataService.GetAllSitesForPlanRegion(planDetailsId, region, "<All>").ToList();
            }
            var obj = (from x in Sites.AsEnumerable().Where(x => x.CityId.Trim()== locationId.Trim())
                       select new
                       {
                           i = x.PlanSiteId,
                           cell = new string[]
                                                 {
                                                     "",
                                                     x.PlanSiteId.ToString(),
                                                     x.SiteNo.ToString(),
                                                     x.SiteName,x.DisplayVendor, x.DisplayVendorName,
                                                     x.Addressline1,
                                                     x.Addressline2, x.Addressline3, x.SiteSize, x.Illumination, x.H.ToString(), x.V.ToString(), x.SiteSizeInSqFt.ToString(), x.SizeRatio, x.MediaType, 
                                                     x.NoOfFaces.ToString(), x.StartDate == null? x.StartDate.ToString() : x.StartDate.Value.ToShortDateString(), 
                                                     x.EndDate == null ? x.EndDate.ToString() : x.EndDate.Value.ToShortDateString(), x.Days.ToString(), x.Qty.ToString()
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

        [HttpPost]
        public ActionResult UpdateSite(FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            var planSite = new PlanSite();
            if (operation != null && operation.Equals("add"))
            {

                if (operation.Equals("add"))
                {
                    planSite = new PlanSite()
                                   {
                                       PlanSiteId = Guid.Parse(formCollection["planSiteId"])
                       // PlanSiteId = Guid.NewGuid()
                    };
                    Sites.Add(planSite);
                }
            }
            else
            {
                planSite =
                    Sites.Where(
                        x =>
                        x.PlanSiteId== Guid.Parse(formCollection["PlanSiteId"])).
                        FirstOrDefault();
            }
            if (planSite != null)
            {                
                planSite.Addressline1 = formCollection["addressline1"];
                planSite.Addressline2 = formCollection["addressline2"];
                planSite.Addressline3 = formCollection["addressline3"];
                planSite.Days = formCollection["Days"] == "" ? 0 : Convert.ToInt32(formCollection["Days"]);
                planSite.DisplayCost = formCollection["DisplayCost"] == null
                                           ? 0
                                           : Convert.ToDecimal(formCollection["DisplayCost"]);
                if (formCollection["DisplayFromDate"] != null)
                    planSite.DisplayFromDate = Convert.ToDateTime(formCollection["DisplayFromDate"]);

                planSite.DisplayRate = formCollection["DisplayRate"] == null
                                           ? 0
                                           : Convert.ToDecimal(formCollection["DisplayRate"]);
                if (formCollection["DisplayToDate"] != null)
                    planSite.DisplayToDate = Convert.ToDateTime(formCollection["DisplayToDate"]);
                if (formCollection["selectedLocationVendor"] != null && formCollection["selectedLocationVendor"] != "")
                    planSite.DisplayVendor = formCollection["selectedLocationVendor"];
                else
                {
                    planSite.DisplayVendor = formCollection["DisplayVendor"];
                }
                planSite.DisplayVendorName = formCollection["DisplayVendorName"];
                if (formCollection["EndDate"] != "" )
                    planSite.EndDate = Convert.ToDateTime(formCollection["EndDate"]);
                planSite.H = formCollection["H"] == "" ? 0 : Convert.ToInt32(formCollection["H"]);
                planSite.Illumination = formCollection["Illumination"];
                planSite.MediaType = formCollection["MediaType"];
                planSite.NoOfFaces = formCollection["NoOfFaces"] == ""
                                         ? 0
                                         : Convert.ToInt32(formCollection["NoOfFaces"]);
                planSite.Qty = formCollection["Qty"] == "" ? 0 : Convert.ToInt32(formCollection["Qty"]);
                planSite.SiteName = formCollection["SiteName"];
                planSite.SiteNo = formCollection["SiteNo"] == "" ? 0 : Convert.ToInt32(formCollection["SiteNo"]);
                planSite.SiteSize = formCollection["SiteSize"];
                planSite.SizeRatio = formCollection["SizeRatio"];
                if (formCollection["StartDate"] != "")
                    planSite.StartDate = Convert.ToDateTime(formCollection["StartDate"]);
                planSite.V = formCollection["V"] == "" ? 0 : Convert.ToInt32(formCollection["V"]);
                planSite.IsDirty = true;
            }
            return Content("Success");
        }

        /* not used..saved viz ajax
        [HttpPost]
        public ActionResult PlanSite(PlanSiteViewModel planSiteViewModel)
        {
            planSiteViewModel.PlanSites = Sites;
            var command = new CreateOrChangeSitesInPlan();
            command.PlanDetailId = planSiteViewModel.PlanDetailsId;
            
            command.Sites = new Dictionary<Guid, PlanSiteDTO>();
            command.DeselectedSites = new Dictionary<Guid, PlanSiteDTO>();
            #region Sites Deselected
            foreach (var site in DeselectedSites)
            {
                Sites.Remove(site);
                command.DeselectedSites.Add(site.PlanSiteId,
                                   new PlanSiteDTO()
                                   {
                                       Addressline1 = site.Addressline1,
                                       Addressline2 = site.Addressline2,
                                       Addressline3 = site.Addressline3,
                                       Days = site.Days,
                                       DisplayCost = site.DisplayCost,
                                       DisplayFromDate = site.DisplayFromDate,
                                       DisplayRate = site.DisplayRate,
                                       DisplayToDate = site.DisplayToDate,
                                       DisplayVendor = site.DisplayVendor,
                                       EndDate = site.EndDate,
                                       H = site.H,
                                       Illumination = site.Illumination,
                                       MediaType = site.MediaType,
                                       NoOfFaces = site.NoOfFaces,
                                       PlanCityId = planSiteViewModel.PlanCityId,
                                       PlanSiteId = site.PlanSiteId,
                                       Qty = site.Qty,
                                       SiteName = site.SiteName,
                                       SiteNo = site.SiteNo,
                                       SiteSize = site.SiteSize,
                                       SizeRatio = site.SizeRatio,
                                       StartDate = site.StartDate,
                                       V = site.V
                                   });
            }
            #endregion Sites Deselected

            #region sites Added
            foreach (var site in Sites)
            {
                command.Sites.Add(site.PlanSiteId,
                                   new PlanSiteDTO()
                                   {
                                       Addressline1 = site.Addressline1,
                                       Addressline2 = site.Addressline2,
                                       Addressline3 = site.Addressline3,
                                       Days = site.Days,
                                       DisplayCost = site.DisplayCost,
                                       DisplayFromDate = site.DisplayFromDate,
                                       DisplayRate = site.DisplayRate,
                                       DisplayToDate = site.DisplayToDate,
                                       DisplayVendor = site.DisplayVendor,
                                       EndDate = site.EndDate,
                                       H = site.H,
                                       Illumination = site.Illumination,
                                       MediaType = site.MediaType,
                                       NoOfFaces = site.NoOfFaces,
                                       PlanCityId = planSiteViewModel.PlanCityId,
                                       PlanSiteId = site.PlanSiteId,
                                       Qty = site.Qty,
                                       SiteName = site.SiteName,
                                       SiteNo = site.SiteNo,
                                       SiteSize = site.SiteSize,
                                       SizeRatio = site.SizeRatio,
                                       StartDate = site.StartDate,
                                       V = site.V
                                   });
            }
            #endregion sites Added

            

            var service = new MyNotesCommandServiceClient();
            service.CreateOrChangeSitesInPlan(command);
            var planById = SharedDataService.GetPlanById(planSiteViewModel.PlanDetailsId);
            planSiteViewModel.BriefNo = planById.BriefNo.ToString();
            planSiteViewModel.Budget = planById.Budget;
            planSiteViewModel.EndDate = planById.EndDate;
            planSiteViewModel.PlanNo = planById.PlanNo;
            planSiteViewModel.StartDate = planById.StartDate;
            planSiteViewModel.CityBudget = planSiteViewModel.CityBudgetH;
            
            planSiteViewModel.Regions = SharedDataService.GetAllRegionsForPlan(planSiteViewModel.PlanDetailsId);
            planSiteViewModel.PlanCities = SharedDataService.GetAllCitiesForPlanRegion(planSiteViewModel.PlanDetailsId,"<All>");
            planSiteViewModel.Region = planSiteViewModel.Region;
            if (planSiteViewModel.Region.Length > 0)
            {
                planSiteViewModel.RegionBudget =
                    planSiteViewModel.Regions.Single(x => x.Region == planSiteViewModel.Region).Budget;
                planSiteViewModel.PlannerId =
                    planSiteViewModel.Regions.Single(x => x.Region == planSiteViewModel.Region).PlannerName;
            }
            Sites = null;
            return View(planSiteViewModel);
        }
        */

        public ActionResult SaveForm(Guid planDetailsId, Guid planCityId)
        {
            try
            {
                var command = new CreateOrChangeSitesInPlan();
                command.PlanDetailId = planDetailsId;

                command.Sites = new Dictionary<Guid, PlanSiteDTO>();
                command.DeselectedSites = new Dictionary<Guid, PlanSiteDTO>();
                #region Sites Deselected
                foreach (var site in DeselectedSites)
                {
                    Sites.Remove(site);
                    PlanSite ps = Sites.Where(x => x.PlanSiteId == site.PlanSiteId).FirstOrDefault();
                    Sites.Remove(ps);
                    command.DeselectedSites.Add(site.PlanSiteId,
                                       new PlanSiteDTO()
                                       {
                                           Addressline1 = site.Addressline1,
                                           Addressline2 = site.Addressline2,
                                           Addressline3 = site.Addressline3,
                                           Days = site.Days,
                                           DisplayCost = site.DisplayCost,
                                           DisplayFromDate = site.DisplayFromDate,
                                           DisplayRate = site.DisplayRate,
                                           DisplayToDate = site.DisplayToDate,
                                           DisplayVendor = site.DisplayVendor,
                                           EndDate = site.EndDate,
                                           H = site.H,
                                           Illumination = site.Illumination,
                                           MediaType = site.MediaType,
                                           NoOfFaces = site.NoOfFaces,
                                           PlanCityId = planCityId,
                                           PlanSiteId = site.PlanSiteId,
                                           Qty = site.Qty,
                                           SiteName = site.SiteName,
                                           SiteNo = site.SiteNo,
                                           SiteSize = site.SiteSize,
                                           SizeRatio = site.SizeRatio,
                                           StartDate = site.StartDate,
                                           V = site.V
                                       });
                }
                #endregion Sites Deselected

                #region sites Added
                foreach (var site in Sites.Where(x => x.IsDirty))
                {
                    command.Sites.Add(site.PlanSiteId,
                                       new PlanSiteDTO()
                                       {
                                           Addressline1 = site.Addressline1,
                                           Addressline2 = site.Addressline2,
                                           Addressline3 = site.Addressline3,
                                           Days = site.Days,
                                           DisplayCost = site.DisplayCost,
                                           DisplayFromDate = site.DisplayFromDate,
                                           DisplayRate = site.DisplayRate,
                                           DisplayToDate = site.DisplayToDate,
                                           DisplayVendor = site.DisplayVendor,
                                           EndDate = site.EndDate,
                                           H = site.H,
                                           Illumination = site.Illumination,
                                           MediaType = site.MediaType,
                                           NoOfFaces = site.NoOfFaces,
                                           PlanCityId = planCityId,
                                           PlanSiteId = site.PlanSiteId,
                                           Qty = site.Qty,
                                           SiteName = site.SiteName,
                                           SiteNo = site.SiteNo,
                                           SiteSize = site.SiteSize,
                                           SizeRatio = site.SizeRatio,
                                           StartDate = site.StartDate,
                                           V = site.V
                                       });
                }
                #endregion sites Added

                if (command.Sites.Count > 0 || command.DeselectedSites.Count > 0)
                {
                    var service = new MyNotesCommandServiceClient();
                    service.CreateOrChangeSitesInPlan(command);
                    DeselectedSites = null;
                    Sites.All(x => x.IsDirty = false);
                }
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
            return Content("Success");
        }

        public ActionResult GetSiteDetails(string SiteNameOrNo, string type)
        {
            string siteDetails = SharedDataService.GetSiteDetails(SiteNameOrNo, type);
            return Content(siteDetails);
        }

        public ActionResult DeleteSite(FormCollection formCollection)
        {
            Guid plansiteId = Guid.Parse(formCollection["planSiteId"]);

            PlanSite planSite = Sites.Where(x => x.PlanSiteId == plansiteId).FirstOrDefault();
            //ViewModel.Plan.PlanSite planSite = Sites[id-1]; 
            List<ViewModel.Plan.PlanSite> sites = DeselectedSites;
            sites.Add(planSite);
            DeselectedSites = sites;
           // Sites.Remove(planSite);
            return Content("Success");
        }

        #region Display Info Change
        public ActionResult IndexForDisplayInfoChange()
        {
            IEnumerable<PlanAddDisplayInfoViewModel> viewModel = SharedDataService.GetAllPlansForDisplayInfoIndex();
            return View(viewModel);
        }

        
        public ActionResult ChangeDisplayInfo(Guid Id)
        {
            PlanAddDisplayInfoViewModel viewModel = SharedDataService.GetPlanDisplayInfoByPlanId(Id);
            Sites = null;
            viewModel.Regions = SharedDataService.GetAllRegionsForPlan(Id);
            return View(viewModel);
        }

        public ActionResult GetSitesForDisplay(Guid planDetailsId, string region, string locationId)
        {
            var pageIndex = 1;
            var pageSize = 1;
            var totalRecords = 1;
            var totalPages = 1;

            if (Sites.Count == 0)
            {
                Sites = SharedDataService.GetAllSitesForPlanRegionForDisplay(planDetailsId, region, locationId).ToList();
            }
            var obj = (from x in Sites.AsEnumerable()
                       select new
                       {
                           i = x.PlanSiteId,
                           cell = new string[]
                                                 {
                                                     "",
                                                     x.IsSelected,
                                                     x.PlanSiteId.ToString(),
                                                     x.SiteNo.ToString(),
                                                     x.SiteName, x.SiteSizeInSqFt.ToString() ,x.DisplayVendor, x.DisplayVendorName,
                                                     x.DisplayRate.ToString(),  x.DisplayCost.ToString() , x.DisplayFromDate.ToString(), x.DisplayToDate.ToString(), 
                                                     x.DisplayClientRate.ToString(), x.DisplayClientCost.ToString(),
                                                     x.Addressline1,
                                                     x.Addressline2, x.Addressline3
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

        [HttpPost]
        public ActionResult UpdateDisplayInfoForSite(FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            var planSite = new PlanSite();
            
            planSite =
                Sites.Where(
                    x =>
                    x.PlanSiteId == Guid.Parse(formCollection["PlanSiteId"])).
                    FirstOrDefault();

            if (planSite != null)
            {
               
                if (formCollection["DisplayFromDate"] != null && formCollection["DisplayFromDate"] != "")
                    planSite.DisplayFromDate = Convert.ToDateTime(formCollection["DisplayFromDate"]);
                
                planSite.DisplayRate = formCollection["DisplayRate"] == null
                                           ? 0
                                           : Convert.ToDecimal(formCollection["DisplayRate"]);

                planSite.DisplayCost = formCollection["DisplayCost"] == null
                                           ? 0
                                           : Convert.ToInt32(Convert.ToDecimal(formCollection["DisplayCost"]) * planSite.SiteSizeInSqFt);

                if (formCollection["DisplayToDate"] != null && formCollection["DisplayToDate"] != "")
                    planSite.DisplayToDate = Convert.ToDateTime(formCollection["DisplayToDate"]);

                if (formCollection["selectedLocationVendor"] != null)
                    planSite.DisplayVendor = formCollection["selectedLocationVendor"];
                else
                {
                    planSite.DisplayVendor = formCollection["DisplayVendor"];
                }

                planSite.DisplayClientRate= formCollection["DisplayClientRate"] == null
                                          ? 0
                                          : Convert.ToDecimal(formCollection["DisplayClientRate"]);

                planSite.DisplayClientCost = formCollection["DisplayClientRate"] == null
                                         ? 0
                                         : Convert.ToDecimal(formCollection["DisplayClientRate"]) * planSite.SiteSizeInSqFt;
                
                planSite.DisplayVendorName = formCollection["DisplayVendorName"];
                planSite.IsSelected = formCollection["IsSelected"].ToString();
                
            }
            return Content("Success");
        }

        public ActionResult SelectAllSites(Guid planDetailsId, string region, string locationId)
        {
            foreach (var planSite in Sites)
            {
                planSite.IsSelected = "Yes";
            }
            return GetSitesForDisplay(planDetailsId, region, locationId);
        }

        public ActionResult SelectNone(Guid planDetailsId, string region, string locationId)
        {
            foreach (var planSite in Sites)
            {
                planSite.IsSelected = "No";
            }
            return GetSitesForDisplay(planDetailsId, region, locationId);
        }

        public ActionResult UpdateDisplayInfo(Guid planDetailsId, string region , string locationId, string VendorName, string VendorId, decimal DisplayRate, string fromDate, string toDate, decimal ClientRate)
        {
            DateTime dtFromdate , dtToDate;
            foreach (var planSite in Sites)
            {
                if (planSite.IsSelected != null && planSite.IsSelected.Equals("Yes"))
                {
                    planSite.DisplayVendor = VendorId;
                    planSite.DisplayVendorName = VendorName;
                    planSite.DisplayRate = DisplayRate;
                    planSite.DisplayCost = Convert.ToInt32(DisplayRate*planSite.SiteSizeInSqFt);
                    if (fromDate != "")
                    {
                        DateTime.TryParse(fromDate, out dtFromdate);
                        planSite.DisplayFromDate = dtFromdate;
                    }
                    if (toDate != "")
                    {
                        DateTime.TryParse(toDate, out dtToDate);
                        planSite.DisplayToDate = dtToDate;
                    }
                    
                    planSite.DisplayClientRate = ClientRate;
                    planSite.DisplayClientCost = ClientRate*planSite.SiteSizeInSqFt;
                }
            }
            return GetSitesForDisplay(planDetailsId, region, locationId);
        }

        [HttpPost]
        public ActionResult ChangeDisplayInfo(PlanAddDisplayInfoViewModel addDisplayInfoViewModel)
        {
            var command = new ChangeDisplayInfoForSite();
            command.PlanDetailsId = addDisplayInfoViewModel.PlanDetailsId;
            command.PlanCityId = addDisplayInfoViewModel.PlanCityId;
            command.Sites= new Dictionary<Guid, PlanSiteDisplayInfoDTO>();
            foreach (var site in Sites)
            {
                command.Sites.Add(site.PlanSiteId, new PlanSiteDisplayInfoDTO(){ PlanSiteId = site.PlanSiteId, DisplayCost = site.DisplayCost, DisplayFromDate = site.DisplayFromDate, DisplayRate = site.DisplayRate, DisplayToDate = site.DisplayToDate, DisplayVendor = site.DisplayVendor, PlanCityId= site.PlanCityId });
            }
            var service = new MyNotesCommandServiceClient();
            service.ChangeSiteDisplayInfo(command);
            addDisplayInfoViewModel = SharedDataService.GetPlanDisplayInfoByPlanId(addDisplayInfoViewModel.PlanDetailsId);
            addDisplayInfoViewModel.Regions = SharedDataService.GetAllRegionsForPlan(addDisplayInfoViewModel.PlanDetailsId);
            return View(addDisplayInfoViewModel);
        }

        #endregion Display Info Change

        #region Mounting Info change

        public ActionResult IndexForMountingInfoChange()
        {
            IEnumerable<PlanAddMountingInfoViewModel> viewModel = SharedDataService.GetAllPlansForMountingInfoIndex();
            return View(viewModel);
        }

        public ActionResult ChangeMountingInfo(Guid Id)
        {
            PlanAddMountingInfoViewModel viewModel = SharedDataService.GetPlanMountingInfoByPlanId(Id);
            Sites = null;
            viewModel.Regions = SharedDataService.GetAllRegionsForPlan(Id);
            return View(viewModel);
        }

        public ActionResult GetSitesForMounting(Guid planDetailsId, string region, string locationId)
        {
            var pageIndex = 1;
            var pageSize = 1;
            var totalRecords = 1;
            var totalPages = 1;

            if (Sites.Count == 0)
            {
                Sites = SharedDataService.GetAllSitesForPlanRegionForMounting(planDetailsId, region, locationId).ToList();
            }
            var obj = (from x in Sites.AsEnumerable()
                       select new
                       {
                           i = x.PlanSiteId,
                           cell = new string[]
                                                 {
                                                      "",
                                                     x.IsSelected,
                                                     x.PlanSiteId.ToString(),
                                                     x.SiteNo.ToString(),
                                                     x.SiteName, x.SiteSizeInSqFt.ToString() ,x.MountingVendor, x.MountingVendorName,
                                                     x.MountingRate.ToString(),  x.MountingCost.ToString() ,
                                                     x.MountingClientCost.ToString(), x.MountingClientCost.ToString(),
                                                     x.Addressline1,
                                                     x.Addressline2, x.Addressline3
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

        [HttpPost]
        public ActionResult UpdateMountingInfoForSite(FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            var planSite = new PlanSite();

            planSite =
                Sites.Where(
                    x =>
                    x.PlanSiteId == Guid.Parse(formCollection["PlanSiteId"])).
                    FirstOrDefault();

            if (planSite != null)
            {
                planSite.MountingCost = formCollection["MountingCost"] == null
                                           ? 0
                                           : Convert.ToDecimal(formCollection["MountingCost"]);


                planSite.MountingRate = formCollection["MountingRate"] == null
                                           ? 0
                                           : Convert.ToDecimal(formCollection["MountingRate"]);

                planSite.MountingCost = formCollection["MountingCost"] == null
                                           ? 0
                                           : Convert.ToDecimal(formCollection["MountingCost"]) * planSite.SiteSizeInSqFt;

                

                if (formCollection["selectedLocationVendor"] != null)
                    planSite.MountingVendor = formCollection["selectedLocationVendor"];
                else
                {
                    planSite.MountingVendor = formCollection["MountingVendor"];
                }

                planSite.MountingClientRate = formCollection["MountingClientRate"] == null
                                          ? 0
                                          : Convert.ToDecimal(formCollection["MountingClientRate"]);

                planSite.MountingClientCost = formCollection["MountingClientRate"] == null
                                         ? 0
                                         : Convert.ToDecimal(formCollection["MountingClientRate"]) * planSite.SiteSizeInSqFt;

                planSite.MountingVendorName = formCollection["MountingVendorName"];
                planSite.IsSelected = formCollection["IsSelected"].ToString();

            }
            return Content("Success");
        }

        public ActionResult SelectAllSitesForMounting(Guid planDetailsId, string region, string locationId)
        {
            foreach (var planSite in Sites)
            {
                planSite.IsSelected = "Yes";
            }
            return GetSitesForMounting(planDetailsId, region, locationId);
        }

        public ActionResult SelectNoneForMounting(Guid planDetailsId, string region, string locationId)
        {
            foreach (var planSite in Sites)
            {
                planSite.IsSelected = "No";
            }
            return GetSitesForMounting(planDetailsId, region, locationId);
        }

        public ActionResult UpdateMountingInfo(Guid planDetailsId, string region, string locationId, string VendorName, string VendorId, decimal MountingRate, decimal ClientRate)
        {
            
            foreach (var planSite in Sites)
            {
                if (planSite.IsSelected != null && planSite.IsSelected.Equals("Yes"))
                {
                    planSite.MountingVendor = VendorId;
                    planSite.MountingVendorName = VendorName;
                    planSite.MountingRate = MountingRate;
                    planSite.MountingCost = MountingRate * planSite.SiteSizeInSqFt;
                    planSite.MountingClientRate = ClientRate;
                    planSite.MountingClientCost = ClientRate * planSite.SiteSizeInSqFt;
                }
            }
            return GetSitesForMounting(planDetailsId, region, locationId);
        }

        [HttpPost]
        public ActionResult ChangeMountingInfo(PlanAddMountingInfoViewModel addMountingInfoViewModel)
        {
            var command = new ChangeMountingInfoForSite();
            command.PlanDetailsId = addMountingInfoViewModel.PlanDetailsId;
            command.PlanCityId = addMountingInfoViewModel.PlanCityId;
            command.Sites = new Dictionary<Guid, PlanSiteMountingInfoDTO>();
            foreach (var site in Sites)
            {
                command.Sites.Add(site.PlanSiteId, new PlanSiteMountingInfoDTO() { PlanSiteId = site.PlanSiteId, MountingCost = site.MountingCost, MountingRate = site.MountingRate, MountingVendor = site.MountingVendor, PlanCityId = site.PlanCityId });
            }
            var service = new MyNotesCommandServiceClient();
            service.ChangeSiteMountingInfo(command);
            addMountingInfoViewModel = SharedDataService.GetPlanMountingInfoByPlanId(addMountingInfoViewModel.PlanDetailsId);
            addMountingInfoViewModel.Regions = SharedDataService.GetAllRegionsForPlan(addMountingInfoViewModel.PlanDetailsId);
            return View(addMountingInfoViewModel);
        }
        #endregion MOunting Info change
    }
}
