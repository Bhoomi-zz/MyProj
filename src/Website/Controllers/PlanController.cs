using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Commands;
using Ncqrs;
using Website.CommandService;
using ReadModel;
using System.Linq;
using Website.ViewModel;
using Website.ViewModel.Plan;
using Website.Services;
using CommonDTOs;

namespace Website.Controllers
{

    [HandleError, Authorize]
    public class PlanController : Controller
    {
        private const string SessionKeyCurrentPage = "PlanController_CurrentPage";

        private const string SessionKeyCurrentRegion = "PlanController_CurrentRegion";
        
        private const string SessionKeyCurrentCity = "PlanController_CurrentCity";
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private List<PlanRegionViewModel> Regions
        {
            get
            {
                if (Session[SessionKeyCurrentRegion] != null)
                    return (List<PlanRegionViewModel>)Session[SessionKeyCurrentRegion];
                else
                    return new List<PlanRegionViewModel>();
            }
            set
            {
                Session[SessionKeyCurrentRegion] = value;
            }
        }

        private List<PlanCityViewModel> Cities
        {
            get
            {
                if (Session[SessionKeyCurrentCity] != null)
                    return (List<PlanCityViewModel>)Session[SessionKeyCurrentCity];
                else
                    return new List<PlanCityViewModel>();
            }
            set
            {
                Session[SessionKeyCurrentCity] = value;
            }
        }

       public ActionResult Index()
        {
           var items = SharedDataService.GetAllPlans();
            return View(items);
        }

        public ActionResult Recent()
        {
            var items = SharedDataService.GetRecentPlans();
            return View("Index" ,items);
        }

        public ActionResult Create()
       {
           var briefViewModel = new PlanCreateViewModel();
           briefViewModel.PlanDetailsId = Guid.NewGuid();
           return View(briefViewModel);
       }

       [HttpPost]
        public ActionResult Create(PlanCreateViewModel viewModel)
       {
           if(viewModel.EndDate < viewModel.StartDate) ModelState.AddModelError("StartDate", "StartDate should be less than EndDate");
           if (ModelState.IsValid)
           {
               var command = new CreatePlan();
               command.BriefNo = viewModel.BriefNo;
               command.Budget = viewModel.Budget;
               command.ClientId = viewModel.ClientId;
               command.CreatedOn = viewModel.CreatedOn;
               command.EndDate = viewModel.EndDate;
               command.HeadPlannerId = viewModel.HeadPlannerId;
               command.PlanDetailsId = viewModel.PlanDetailsId;
               command.PlanNo = viewModel.PlanNo;
               command.StartDate = viewModel.StartDate;
               try
               {
                   var service = new MyNotesCommandServiceClient();
                   service.CreatePlan(command);
               }
               catch (Exception ex)
               {
                   _log.Error(ex.Message, ex);
                   throw;
               }
               return RedirectToAction("Index");
           }
           else
           {
               return View(viewModel);
           }
       }

       public ActionResult Edit(Guid id)
       {
           var planEditViewModel = SharedDataService.GetPlanById(id);
           return View(planEditViewModel);
       }

       [HttpPost]
       public ActionResult Edit(PlanEditViewModel viewModel)
       {
           try
           {
               if(viewModel.EndDate < viewModel.StartDate) ModelState.AddModelError("StartDate", "StartDate should be less than EndDate");
               if (ModelState.IsValid)
               {
                   var service = new MyNotesCommandServiceClient();
                   var editPlan = new EditPlan();
                   editPlan.BriefNo = viewModel.BriefNo;
                   editPlan.Budget = viewModel.Budget;
                   editPlan.ClientId = viewModel.ClientId;
                   editPlan.CreatedOn = viewModel.CreatedOn;
                   editPlan.EndDate = viewModel.EndDate;
                   editPlan.HeadPlannerId = viewModel.HeadPlannerId;
                   editPlan.PlanDetailsId = viewModel.PlanDetailsId;
                   editPlan.PlanNo = viewModel.PlanNo;
                   editPlan.StartDate = viewModel.StartDate;
                   service.ChangePlan(editPlan);
                   return RedirectToAction("Index");
               }
               else
               {
                   return View(viewModel);
               }
           }
           catch(Exception exception)
           {
               _log.Error(exception.Message, exception);
               throw;
           }
       }
       
        public ActionResult PlanRegionIndex()
        {
            IEnumerable<PlanEditViewModel> plans = SharedDataService.GetAllPlansForPlanRegionIndex();
            return View(plans);
        }

        public ActionResult PlanRegionsMainView(Guid Id)
        {

            try
            {
                var planEditViewModel = SharedDataService.GetPlanById(Id);
                Regions = null;
               // planEditViewModel.PlanRegions = SharedDataService.GetAllRegionsForPlan(Id);
                return View(planEditViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

       public ActionResult GetRegions(string sidx, string sord, int page, int rows, string PlanDetailsId)
       {
           try
           {
               Guid guid = new Guid(PlanDetailsId);
               var pageIndex = 1;
               var pageSize = 1;
               var totalRecords = 1;
               var totalPages = 1;

               if (Regions.Count == 0)
               {
                   Regions = SharedDataService.GetAllRegionsForPlan(guid).ToList();
               }
               var obj = (from x in Regions.AsEnumerable()
                          select new
                                     {
                                         i = x.PlanRegionId,
                                         cell = new string[]
                                                    {
                                                        "",
                                                        x.PlanRegionId.ToString(),
                                                        x.Region,
                                                        x.PlannerName == null ? "" : x.PlannerName.ToString(),
                                                        x.Budget.ToString()
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
           catch(Exception e)
           {
               _log.Error(e.Message, e);
               throw;
               return Json("");
           }
       }

       [HttpPost]
       public ActionResult UpdateRegions(FormCollection formCollection)
       {
           var operation = formCollection["oper"];
           PlanRegionViewModel planRegion = new PlanRegionViewModel();
           if (operation != null && operation.Equals("add"))
           {

               if (operation.Equals("add"))
               {
                   planRegion = new PlanRegionViewModel()
                   {
                       PlanRegionId =  Guid.Parse(formCollection["PlanRegionId"]) //Guid.NewGuid()
                   };
                   Regions.Add(planRegion);
               }
           }
           else
           {
               planRegion =
                   Regions.Where(
                       x =>
                       x.PlanRegionId == Guid.Parse(formCollection["PlanRegionId"])).
                       FirstOrDefault();
           }
           if (planRegion != null)
           {
               planRegion.Budget = formCollection["Budget"] == null || formCollection["Budget"]==""
                                                     ? 0
                                                     : Convert.ToDecimal(formCollection["Budget"]);

               //planRegion.PlannerName = formCollection["PlannerName"];
               planRegion.PlannerId = formCollection["PlannerId"].Trim();
               planRegion.Region = formCollection["Region"];
               planRegion.IsDirty = true;
           }
           return Content("Success");
       }
       
       public ActionResult SaveRegions(Guid planDetailsId)
       {
           try
           {
               //PlanRegions = Regions;
               var command = new CreateRegionInPlan();
               command.PlanDetailId = planDetailsId;
               command.Regions = new Dictionary<Guid, PlanRegionsDTO>();
               foreach (var region in Regions.Where(x => x.IsDirty))
               {
                   command.Regions.Add(region.PlanRegionId,
                                       new PlanRegionsDTO()
                                           {
                                               Budget = region.Budget,
                                               PlannerId = region.PlannerId,
                                               PlanRegionId = region.PlanRegionId,
                                               Region = region.Region
                                           });
               }
               if (command.Regions.Count > 0)
               {
                   var service = new MyNotesCommandServiceClient();
                   service.CreateRegionInPlan(command);
               }
               //planEditViewModel = SharedDataService.GetPlanById(planEditViewModel.PlanDetailsId);
               //Regions = null;
           }
           catch(Exception e)
           {
               _log.Error(e.Message, e);
               throw;
           }
           return Content("Successs"); //View("PlanRegionsMainView", planEditViewModel);
       }

       public ActionResult PlanRegionCity(Guid Id)
       {
           var planModel = SharedDataService.GetPlanById(Id);
           Cities = null;
           var planCityMainViewModel = new PlanCityMainViewModel() { BriefNo = planModel.BriefNo, Budget = planModel.Budget, CreatedOn = planModel.CreatedOn, EndDate = planModel.EndDate,  PlanDetailsId = planModel.PlanDetailsId, PlanNo = planModel.PlanNo, StartDate = planModel.StartDate, HeadPlannerName = planModel.HeadPlannerName};
           planCityMainViewModel.Regions = SharedDataService.GetAllRegionsForPlan(Id);
           return View(planCityMainViewModel);
       }
        
       public ActionResult SaveCities(Guid planDetailsId)
       {
           try
           {
           //planCityMainViewModel.PlanCities = Cities;
           var command = new CreateOrChangeCitiesInPlan();
           command.PlanDetailId = planDetailsId;
           command.Cities = new Dictionary<Guid, PlanCityDTO>();
           foreach (var city in Cities.Where(x=> x.IsDirty))
           {
               command.Cities.Add(city.PlanCitiesId,
                                  new PlanCityDTO()
                                      {
                                          Budget = city.Budget,
                                          PlannerId = city.PlannerId,
                                          PlanCitiesId = city.PlanCitiesId,
                                          Region = city.Region,
                                          LocationId = city.LocationId
                                      });
           }
           if (command.Cities.Count > 0)
           {
               var service = new MyNotesCommandServiceClient();
               service.CreateOrChangeCitiesInPlan(command);
           }
          // var planById = SharedDataService.GetPlanById(planDetailsId);
           //planCityMainViewModel.BriefNo = planById.BriefNo;
           //    planCityMainViewModel.Budget = planById.Budget;
           //    planCityMainViewModel.CreatedOn = planById.CreatedOn;
           //    planCityMainViewModel.EndDate = planById.EndDate;
           //    planCityMainViewModel.HeadPlannerName = planById.HeadPlannerName;
           //    planCityMainViewModel.PlanNo = planById.PlanNo;
           //    planCityMainViewModel.StartDate = planById.StartDate;
           //    planCityMainViewModel.Regions =
           //        SharedDataService.GetAllRegionsForPlan(planCityMainViewModel.PlanDetailsId);
           return Content("Success"); //View(planCityMainViewModel);
           }
           catch (Exception e)
           {
               _log.Error(e.Message, e);
               return Content("Failure"); 
           }
       }
        public ActionResult GetCities(string sidx, string sord, int page, int rows, string PlanDetailsId, string region)
       {
           Guid guid = new Guid(PlanDetailsId);
           var pageIndex = 1;
           var pageSize = 1;
           var totalRecords = 1;
           var totalPages = 1;


           if (Cities.Count == 0 || Cities.Where(x => x.Region==region).Count()==0)
           {
               Cities = SharedDataService.GetAllCitiesForPlanRegion(guid, region).ToList() ;
           }

           var obj = (from x in Cities.AsEnumerable()
                      select new
                      {
                          i = x.PlanCitiesId,
                          cell = new string[]
                                                 {
                                                     "",
                                                     x.PlanCitiesId.ToString(),
                                                     x.Region,
                                                     x.LocationName,
                                                     x.PlannerName== null ? "": x.PlannerName.ToString(),
                                                     x.Budget.ToString()
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
        public ActionResult UpdateCity(FormCollection formCollection)
       {
           var operation = formCollection["oper"];
           var planCity = new PlanCityViewModel();
           if (operation != null && operation.Equals("add"))
           {

               if (operation.Equals("add"))
               {
                   planCity = new PlanCityViewModel()
                                  {
                                      PlanCitiesId = Guid.Parse(formCollection["PlanCitiesId"])
                   };
                   Cities.Add(planCity);
               }
           }
           else
           {
               planCity =
                   Cities.Where(
                       x =>
                       x.PlanCitiesId == Guid.Parse(formCollection["PlanCitiesId"])).
                       FirstOrDefault();
           }
           if (planCity != null)
           {
               planCity.Budget = formCollection["Budget"] == null || formCollection["Budget"] == ""
                                                     ? 0
                                                     : Convert.ToDecimal(formCollection["Budget"]);

               //planRegion.PlannerName = formCollection["PlannerName"];
               planCity.PlannerId = formCollection["PlannerId"].Trim();
               planCity.Region = formCollection["Region"];

               planCity.PlannerName = formCollection["PlannerName"] ?? planCity.PlannerName;
               
               if (formCollection["selectedLocationId"] != "NotAssigned")
               {
                   planCity.LocationId = formCollection["selectedLocationId"];
                   planCity.LocationName = formCollection["LocationName"];
               }
               planCity.IsDirty = true;
           }
           return Content("Success");
       }
        public ActionResult CreatePlanWithBriefNo(int briefNo, string contactId, string contactName)
        {
            var briefViewModel = new PlanCreateViewModel();
            briefViewModel.PlanDetailsId = Guid.NewGuid();
            briefViewModel.ClientId = contactId;
            briefViewModel.ClientName = contactName;    
            briefViewModel.BriefNo = briefNo;
            briefViewModel.CreatedOn = DateTime.Now;
            return View("Create", briefViewModel);
        }
        public ActionResult CopyBriefAllocation(Guid planid)
        {
            var alocRegions = SharedDataService.GetAllocatedRegionAndCitiesForBriefByPlanNo(planid);
            var regions = Regions;
            foreach (var v in alocRegions)
            {
                if (regions.Find(x => x.Region == v.Region) == null)
                {
                    regions.Add(new PlanRegionViewModel() {PlanRegionId = Guid.NewGuid(), Region= v.Region, IsDirty=true, Budget = v.Budget, PlannerId = v.PlannerId, PlannerName = v.PlannerName });
                }
            }
            Regions = regions;
            return Content("Success");
        }
        public ActionResult CopyBriefAllocationCities(Guid planId, string Region)
        {
            var aloccities = SharedDataService.GetAllocatedRegionAndCitiesForBriefByPlanNo(planId);
            var cities = Cities;
            foreach (var v in aloccities)
            {
                if (cities.Find(x => x.LocationId== v.LocationId) == null)
                {
                    if(v.Region == Region )
                        cities.Add(new PlanCityViewModel() { PlanCitiesId = Guid.NewGuid(), LocationName = v.LocationName, LocationId = v.LocationId, PlannerId = v.PlannerId, PlannerName = v.PlannerName, Region = v.Region, IsDirty = true, Budget = v.Budget });
                }
            }
            Cities = cities;
            return Content("Success");
        }
    }
}
