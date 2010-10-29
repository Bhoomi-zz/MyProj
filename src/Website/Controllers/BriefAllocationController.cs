using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using JQGridMVCDemo.Helper;
using MvcGridView.Code;
using Ncqrs;
using ReadModel;
using Website.CommandService;
using Website.Services;
using Website.ViewModel;
using Microsoft.Web.Mvc;
using Commands;
using CommonDTOs;

namespace Website.Controllers
{
    
    [HandleError, Authorize]
    public class BriefAllocationController : Controller
    {
        private const string SessionKeyCurrentPage = "BriefAllocationController_CurrentPage";
        private const string SessionKeyCurrentRegionAndCity = "BriefAllocationController_CurrentRegionAndCity";
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<RegionsAndCitiesViewModel> RegionsAndCities
        {
            get
            {
                if (Session[SessionKeyCurrentRegionAndCity] != null)
                    return (List<RegionsAndCitiesViewModel>)Session[SessionKeyCurrentRegionAndCity];
                else
                    return new List<RegionsAndCitiesViewModel>();
            }
            set
            {
                Session[SessionKeyCurrentRegionAndCity] = value;
            }
        }

        public ActionResult Index()
        {
            Session.Remove(SessionKeyCurrentRegionAndCity);
            IList<BriefAllocationViewModelForIndex> items = new List<BriefAllocationViewModelForIndex>();
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.BriefAllocations
                             join user in context.bmUsers
                             on item.HeadPlannerID equals user.sUserID
                             join brief in context.Briefs
                             on item.BriefNo equals brief.sBriefNo
                             join contact in context.bmContacts
                             on brief.sContactIDFK equals contact.sContactId
                             orderby item.CreatedOn
                             select new { item.BriefAllocationId, item.CreatedOn, HeadPlanner = user.sUserName, item.BriefNo, brief.sContactIDFK, contact.sName });

                foreach (var item in query)
                {
                    var index = new BriefAllocationViewModelForIndex();
                    if (item.CreatedOn != null)
                        index.CreatedOn = (DateTime)item.CreatedOn;
                    index.BriefAllocationId = item.BriefAllocationId;
                    index.BriefNo = item.BriefNo;
                    index.HeadPlanner = item.HeadPlanner;
                    index.contactId = item.sContactIDFK;
                    index.ClientName = item.sName;
                    items.Add(index);
                }
            }
            return View(items);
        }

        public ActionResult Create()
        {
            var briefAllocationViewModelForCreate = new BriefAllocationViewModelForCreate();
            briefAllocationViewModelForCreate.PlanId = Guid.NewGuid();
            briefAllocationViewModelForCreate.CreatedOn = DateTime.Now;
            ViewData["Users"] = briefAllocationViewModelForCreate.Users;
            return View(briefAllocationViewModelForCreate);
        }

        [HttpPost]
        public ActionResult Create(BriefAllocationViewModelForCreate briefAllocationViewModelForCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(briefAllocationViewModelForCreate);
                if (RegionsAndCities.Count == 0)
                    return View(briefAllocationViewModelForCreate);
                briefAllocationViewModelForCreate.RegionAndCities = RegionsAndCities;
                var briefAllocation = new CreateBriefAllocation() { PlanId = briefAllocationViewModelForCreate.PlanId, BriefNo = briefAllocationViewModelForCreate.BriefNo, CreatedOn = briefAllocationViewModelForCreate.CreatedOn, HeadPlannerId = briefAllocationViewModelForCreate.HeadPlannerId };
                IEnumerator<RegionsAndCitiesViewModel> iEnum = RegionsAndCities.GetEnumerator();
                while (iEnum.MoveNext())
                {
                    if (briefAllocation.RegionCities == null)
                        briefAllocation.RegionCities = new List<RegionsAndCitiesDTO>();

                    briefAllocation.RegionCities.Add(new RegionsAndCitiesDTO()
                                                         {
                                                             RegionsAndCitiesId = iEnum.Current.RegionsAndCitiesId,
                                                             LocationId = iEnum.Current.LocationId,
                                                             PlannerId = iEnum.Current.PlannerId,
                                                             Region = iEnum.Current.Region,
                                                             Budget = iEnum.Current.Budget
                                                         });
                }
                var service = new MyNotesCommandServiceClient();

                service.CreateBriefAllocation(briefAllocation);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                _log.Error(e.Message, e);
                return View();
            }
        }

        public ActionResult Edit( Guid id)
        {
            BriefAllocation briefAllocation;
            //AllocatedRegionAndCity allocatedRegionAndCity;
            //var briefAllocationViewModelForEdit = new BriefAllocationViewModelForEdit();

            //using (var context = new MyNotesReadModelEntities())
            //{
            //    briefAllocation = context.BriefAllocations.Single(item => item.BriefAllocationId == id);
            //    var allocatedregionsAndcities = (from regionAndCity in context.AllocatedRegionAndCities
            //                                     where regionAndCity.BriefAllocationId == id
            //                                     select regionAndCity);
            //    IEnumerable<RegionsAndCitiesViewModel> regionsAndCities =
            //        allocatedregionsAndcities.Select(
            //            rc =>
            //            new RegionsAndCitiesViewModel
            //                {
            //                    RegionsAndCitiesId = rc.AllocatedRegionAndCitiesId,
            //                    LocationId = rc.LocationID,
            //                    Region = rc.Region,
            //                    Budget = (rc.budget == null ? 0 : (decimal)(rc.budget)),
            //                    PlannerId = rc.PlannerID
            //                });
            //    briefAllocationViewModelForEdit.RegionAndCities = regionsAndCities.ToList();
            //    RegionsAndCities = briefAllocationViewModelForEdit.RegionAndCities;
            //}
            //briefAllocationViewModelForEdit.RegionAndCities = SharedDataService.GetAllocatedRegionAndCitiesForBrief(id);
            //briefAllocationViewModelForEdit.PlanId = briefAllocation.BriefAllocationId;
            //briefAllocationViewModelForEdit.BriefNo = (briefAllocation.BriefNo == null ? 0 : Convert.ToInt32(briefAllocation.BriefNo));
            //briefAllocationViewModelForEdit.HeadPlannerId = briefAllocation.HeadPlannerID;
            //if (briefAllocation.CreatedOn != null)
            //    briefAllocationViewModelForEdit.CreatedOn = Convert.ToDateTime(briefAllocation.CreatedOn);
            var briefAllocationViewModelForEdit = SharedDataService.GetAllocatedBrief(id);
            RegionsAndCities = briefAllocationViewModelForEdit.RegionAndCities;
            return View(briefAllocationViewModelForEdit);
        }

        [HttpPost]
        public ActionResult Edit(BriefAllocationViewModelForEdit briefAllocationViewModelForEdit)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(briefAllocationViewModelForEdit);
                if (RegionsAndCities.Count == 0)
                    return View(briefAllocationViewModelForEdit);
                briefAllocationViewModelForEdit.RegionAndCities = RegionsAndCities;
                var briefAllocation = new EditBriefAllocation() { PlanId = briefAllocationViewModelForEdit.PlanId, BriefNo = briefAllocationViewModelForEdit.BriefNo, CreatedOn = briefAllocationViewModelForEdit.CreatedOn, HeadPlannerId = briefAllocationViewModelForEdit.HeadPlannerId };
                IEnumerator<RegionsAndCitiesViewModel> iEnum = RegionsAndCities.GetEnumerator();
                while (iEnum.MoveNext())
                {
                    if (briefAllocation.RegionCities == null)
                        briefAllocation.RegionCities = new List<RegionsAndCitiesDTO>();

                    briefAllocation.RegionCities.Add(new RegionsAndCitiesDTO()
                    {
                        RegionsAndCitiesId = iEnum.Current.RegionsAndCitiesId,
                        LocationId = iEnum.Current.LocationId,
                        PlannerId = iEnum.Current.PlannerId,
                        Region = iEnum.Current.Region,
                        Budget = iEnum.Current.Budget
                    });
                }
                var service = new MyNotesCommandServiceClient();

                service.ChangeBriefAllocation(briefAllocation);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                _log.Error(e.Message, e);
                return View();
            }
        }

        public ActionResult GetRegionAndCities(string sidx, string sord, int page, int rows, string PlanId)
        {
            Guid guid = new Guid(PlanId);
            var pageIndex = 1;
            var pageSize = 1;
            var totalRecords = 1;
            var totalPages = 1;

            RegionsAndCitiesViewModel briefAllocationViewModel;
            if (RegionsAndCities.Count == 0)
            {
               // var sharedDataService = new SharedDataService();
                RegionsAndCities = SharedDataService.GetAllocatedRegionAndCitiesForBrief(guid);
            }

            var obj = (from x in RegionsAndCities.AsEnumerable()
                       select new
                       {
                           i = x.RegionsAndCitiesId,
                           cell = new string[]
                                                 {
                                                     "",
                                                     x.RegionsAndCitiesId.ToString(),
                                                     x.Region,
                                                     x.LocationName,
                                                     x.PlannerName == null ? "": x.PlannerName.Trim(),
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
        public ActionResult UpdateRegionAndCity(FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            RegionsAndCitiesViewModel briefAllocationViewModel = null;
            if (operation != null && operation.Equals("add"))
            {

                if (operation.Equals("add"))
                {
                    briefAllocationViewModel = new RegionsAndCitiesViewModel()
                                                   {
                                                       RegionsAndCitiesId = Guid.Parse(formCollection["RegionsAndCitiesId"]) //Guid.NewGuid()
                                                   };
                    RegionsAndCities.Add(briefAllocationViewModel);
                }
            }
            else
            {
                briefAllocationViewModel =
                    RegionsAndCities.Where(
                        x =>
                        x.RegionsAndCitiesId == Guid.Parse(formCollection["RegionsAndCitiesId"])).
                        FirstOrDefault();
            }
            if (briefAllocationViewModel != null)
                {
                    briefAllocationViewModel.Budget = formCollection["Budget"] == null || formCollection["Budget"] == ""
                                                          ? 0
                                                          : Convert.ToDecimal(formCollection["Budget"]);
                    if (formCollection["selectedLocationId"] != "NotAssigned")
                    {
                            briefAllocationViewModel.LocationId = formCollection["selectedLocationId"] ;
                            briefAllocationViewModel.LocationName = formCollection["LocationName"];
                    }
                    briefAllocationViewModel.PlannerName = formCollection["PlannerName"];
                    briefAllocationViewModel.PlannerId = formCollection["PlannerId"].Trim();
                    briefAllocationViewModel.Region = formCollection["Region"];
                }
                return Content("Success");
            }

        public ActionResult AllocateBrief(int briefNo)
        {
            var briefAllocationViewModelForCreate = new BriefAllocationViewModelForCreate();
            briefAllocationViewModelForCreate.PlanId = Guid.NewGuid();
            briefAllocationViewModelForCreate.BriefNo = briefNo;
            briefAllocationViewModelForCreate.CreatedOn = DateTime.Now;
            ViewData["Users"] = briefAllocationViewModelForCreate.Users;
            return View("Create", briefAllocationViewModelForCreate);
        }
    }
}

//prmNames: {
//              briefId: "ab"
//          },


 //{ name: 'LocationName', index: 'LocationName', width: 300, align: 'left', editable: true, edittype: 'text',
 //                                     editoptions: { dataInit: function (elem) {
 //                                         setTimeout(function () {
 //                                             $(elem).autocomplete('/Shared/FindLocation/', {
 //                                                 dataType: "json",
 //                                                 multiple: false,
 //                                                 formatItem: function (item, index, total, query) {
 //                                                     return item.value;
 //                                                 },
 //                                                 parse: function (data) {
 //                                                     return $.map(data, function (item) {
 //                                                         return {
 //                                                             data: item,
 //                                                             value: item.Key,
 //                                                             result: item.value
 //                                                         };
 //                                                     });
 //                                                 }
 //                                             }).result(function (event, row) {
 //                                                 $("#LocationId").val(row.Key);
 //                                             });
 //                                         }, 100);
 //                                     }
 //                                     }, editrules: { required: true }
 //                                 },


//<table id ="RegionAndCity" class="scroll" cellpadding="0" cellspacing="0"></table>             
    
//             <div id="listPager" class="scroll" style="text-align:center;"></div>                                                            
//                <div id="listPsetcols" class="scroll" style="text-align:center;"></div>  
//                </div>
//                 <p>
//                <input type="submit" value="Create" />
//            </p>

//.navGrid('#listPager', { edit: true, add: true, del: true, refresh: true },
//                updateDialog,
//                updateDialog,
//                updateDialog)


//se = "<input style='height:22px;width:20px;' type='button' value='S' onclick=\"jQuery('#RegionAndCity').saveRow('" + cl + "');\" />";



//var updateDialog = {
//            url: '<%= Url.Action("UpdateRegionAndCity", "BriefAllocation") %>'
//                , closeAfterAdd: true
//                , closeAfterEdit: true
//                , recreateForm: true
//                , modal: false,
//            onclickSubmit: function (params) {
//                alert('updating')
//                var ajaxData = {};
//                var list = $("#RegionAndCity");
//                var selectedRow = list.getGridParam("selrow");
//                rowData = list.getRowData(selectedRow);
//                var plannerName = $("#PlannerId :selected").text();
//                //ajaxData = { PlanId: PlanId, PlannerName: plannerName, LocationName: $("#LocationId :selected").text() };                
//                ajaxData = { PlanId: PlanId, PlannerName: plannerName, selectedLocationId: selectedLocationIdvar };
//                return ajaxData;
//            }
//        };