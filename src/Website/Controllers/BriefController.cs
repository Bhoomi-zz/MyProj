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

namespace Website.Controllers
{
    
    [HandleError, Authorize]
    public class BriefController : Controller
    {
        //
        // GET: /Brief/
       
        public IEnumerable<SelectListItem> Customers;
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       // private Services.SharedDataService _sharedDataService;
        public ActionResult Index()
        {
            Session["ActiveObjectName"] = "Brief Index";
            IList<BriefViewModelIndex> items = new List<BriefViewModelIndex>();

            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.Briefs
                            join contact in context.bmContacts
                            on item.sContactIDFK equals contact.sContactId
                            where item.IsInActive == false
                             orderby item.dtVoucher descending 
                            select new { BriefId = item.sPlanVersionsCSID, BriefDate= item.dtVoucher, Customer = contact.sName, BriefNo = item.sBriefNo });

                foreach (var item in query)
                {
                    var index = new BriefViewModelIndex();
                    index.BriefDate = item.BriefDate;
                    index.BriefId = item.BriefId;
                    index.BriefNo = item.BriefNo;
                    index.Customer = item.Customer;
                    items.Add(index);
                }
            }
            //IEnumerable<BriefViewModelIndex> items1 = items.ToArray();
            return View(items);
        }

        public ActionResult Add()
        {

            var briefViewModel = new BriefViewModel();
            briefViewModel.BriefDate = System.DateTime.Now;
            return View("AddBrief", briefViewModel);
        }

        [HttpPost]
        public ActionResult Add(BriefViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("AddBrief", viewModel);
                var service = new MyNotesCommandServiceClient();
                var command = new CreateBrief();
                command.BriefDate = viewModel.BriefDate;
                command.Customer = viewModel.Customer;
                command.AdditionalInfo = viewModel.AdditionalInfo;
                command.BudgetValue = viewModel.BudgetValue;
                command.ContactPerson = viewModel.ContactPerson;
                command.ContactPersonId = viewModel.ContactPersonId;
                command.ClientBusinessProfile = viewModel.ClientBusinessProfile;
                command.BudgetTypes = viewModel.BudgetTypes;
                command.IsApproved = viewModel.IsApproved;
                command.ApprovalNo = viewModel.ApprovalNo;
                command.ApprovalDate = viewModel.ApprovalDate;
                command.DeadLineDate = viewModel.DeadLineDate;
                command.Brand1 = viewModel.Brand1;
                command.TentativeStartDate = viewModel.TentativeStartDate;
                command.TentativeEndDate = viewModel.TentativeEndDate;
                command.NoOfYears = viewModel.NoOfYears;
                command.OtherStCommision = viewModel.OtherStCommision;
                command.MarketingObjective = viewModel.MarketingObjective;
                command.RequestSentToHO = viewModel.RequestSentToHO;
                command.CompetitionBrand1 = viewModel.CompetitionBrand1;
                command.CompetitionBrand2 = viewModel.CompetitionBrand2;
                command.PreparedBy = viewModel.PreparedBy;
                command.BriefNo = viewModel.BriefNo;
                command.Brand2 = viewModel.Brand2;
                command.NoOfMonths = viewModel.NoOfMonths;
                command.NoOfDays = viewModel.NoOfDays;
                command.BriefStatus = viewModel.BriefStatus;
                command.OsDays = viewModel.OsDays;
                command.DateCreated = viewModel.DateCreated;
                command.Brand3 = viewModel.Brand3;
                command.CompetitionBrand3 = viewModel.CompetitionBrand3;
                command.SendDate = viewModel.SendDate;
                command.HeadPlannerUserId = viewModel.HeadPlannerUserId;
                command.InPrintFl = viewModel.InPrintFl;
                command.AdObjective = viewModel.AdObjective;
                command.Brand4 = viewModel.Brand4;
                command.LayoutAdapLimitations = viewModel.LayoutAdapLimitations;
                command.InPrintBl = viewModel.InPrintBl;
                command.CompetitionBrand4 = viewModel.CompetitionBrand4;

                command.InPainting = viewModel.InPainting;
                command.TargetGroupBrand1 = viewModel.TargetGroupBrand1;
                command.TypeOfVehicles = viewModel.TypeOfVehicles;
                command.TgsMedia1 = viewModel.TgsMedia1;

                command.PreferredSize = viewModel.PreferredSize;
                command.Others = viewModel.Others;
                command.AdMix = viewModel.AdMix;
                command.DisplayType = viewModel.DisplayType;
                command.TargetGroupBrand2 = viewModel.TargetGroupBrand2;
                command.TgsMedia2 = viewModel.TgsMedia2;
                command.DisplayTypeSel = viewModel.DisplayTypeSel;
                command.TgsMedia3 = viewModel.TgsMedia3;
                command.TargetGroupBrand3 = viewModel.TargetGroupBrand3;
                command.TgsMedia4 = viewModel.TgsMedia4;
                command.TargetGroupBrand4 = viewModel.TargetGroupBrand4;
                command.OutDoorObjective = viewModel.OutDoorObjective;
                command.Market1 = viewModel.Market1;
                command.Market2 = viewModel.Market2;
                command.Market3 = viewModel.Market3;
                command.Market4 = viewModel.Market4;
                command.InVinyl = viewModel.InVinyl;
                command.Complete = viewModel.Complete;
                command.PartVinyl = viewModel.PartVinyl;
                command.Ratio11 = viewModel.Ratio11;
                command.Ratio12 = viewModel.Ratio12;
                command.Ratio13 = viewModel.Ratio13;
                command.Ratio14 = viewModel.Ratio14;
                command.Ratio34 = viewModel.Ratio34;
                command.RatioAll = viewModel.RatioAll;
                command.BillBoard = viewModel.BillBoard;
                command.Buses = viewModel.Buses;
                command.PoleKiosks = viewModel.PoleKiosks;
                command.BusQShelter = viewModel.BusQShelter;
                command.TransitMedia = viewModel.TransitMedia;
                command.PublicUtitities = viewModel.PublicUtitities;
                command.OtherMediaVehicle = viewModel.OtherMediaVehicle;
                command.IsInActive = viewModel.IsInActive;
                command.ContactPerson = command.ContactPersonId;
                service.CreateNewBrief(command);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        public ActionResult Edit(string id)
        {
            ptmPlanVersionsC brief;

            using (var context = new MyNotesReadModelEntities())
            {
                brief = context.Briefs.Single(item => item.sPlanVersionsCSID == id);
            }

// ReSharper disable UseObjectOrCollectionInitializer
            var briefViewModel = new BriefViewModel();
// ReSharper restore UseObjectOrCollectionInitializer
            briefViewModel.BriefId = new Guid(id);
            //if (brief.dtVoucher != null)
                briefViewModel.BriefDate = (DateTime)brief.dtVoucher;
            briefViewModel.AdditionalInfo = briefViewModel.AdditionalInfo;
            briefViewModel.Customer = brief.sContactIDFK;
            briefViewModel.ContactPerson = briefViewModel.GetContactPersonName(brief.sCPersonIDFK);
            briefViewModel.ContactPersonId = brief.sCPersonIDFK;
            briefViewModel.BudgetValue = (Decimal) (brief.nBudgetValue ?? 0);
            briefViewModel.ClientBusinessProfile = brief.sClientBusiProfile ;
            briefViewModel.BudgetTypes = brief.sBudgetTypes == null? "" : brief.sBudgetTypes.Trim() ;
            briefViewModel.IsApproved = brief.sApprovedPlan == "1" ? true : false ;
            briefViewModel.ApprovalNo =  Convert.ToInt32(brief.sApprovalNo ?? "0") ;
            //if (briefViewModel.ApprovalDate != null)
                briefViewModel.ApprovalDate = brief.dApproval ;
            //if (briefViewModel.DeadLineDate != null)
                briefViewModel.DeadLineDate = brief.dDeadlineDate ;
            briefViewModel.Brand1 = brief.sBrand1 ;
            //if (briefViewModel.TentativeStartDate != null)
                briefViewModel.TentativeStartDate = brief.dTentativeStartDate ;
            //if (briefViewModel.TentativeEndDate != null)
                briefViewModel.TentativeEndDate = brief.dTentativeEndDate ;
            briefViewModel.NoOfYears = Convert.ToInt32(brief.nNoOfYears ?? 0); 
            briefViewModel.OtherStCommision = Convert.ToInt32(brief.nOtherSTAgencyComm ?? 0); 
            briefViewModel.MarketingObjective = brief.sMarketingObjective ;
            //if (briefViewModel.RequestSentToHO != null)
                briefViewModel.RequestSentToHO = brief.dRequestSentTOHo ;
            briefViewModel.CompetitionBrand1 = brief.sCompetitionBrand1 ;
            briefViewModel.CompetitionBrand2 =brief.sCompetitionBrand2 ;
            briefViewModel.BriefNo = Convert.ToInt32(brief.sBriefNo ?? "0");
            briefViewModel.Brand2 = brief.sBrand2 ;
            briefViewModel.NoOfMonths = Convert.ToInt32(brief.nNoOfMonths ?? 0); ;
            briefViewModel.NoOfDays = Convert.ToInt32(brief.nNoOfDays ?? 0);
            briefViewModel.BriefStatus = Convert.ToInt32(brief.nBriefStatus ?? 0);
            briefViewModel.OsDays = Convert.ToInt32(brief.nOSDays ?? 0); 
            //if (briefViewModel.DateCreated != null)
                briefViewModel.DateCreated = brief.dtSave ;
            briefViewModel.Brand3 = brief.sBrand3 ;
            briefViewModel.CompetitionBrand3 = brief.sCompetitionBrand3 ;
           // if (briefViewModel.SendDate != null)
                briefViewModel.SendDate = brief.dtSend ;
            if (brief.sHeadPlannerUserIDFK!= null)
            briefViewModel.HeadPlannerUserId = new Guid(brief.sHeadPlannerUserIDFK) ;
            briefViewModel.InPrintFl = Convert.ToBoolean(brief.nInPrintFL ?? false); 
            briefViewModel.AdObjective = brief.sAdObjective ;
            briefViewModel.Brand4 = brief.sBrand4 ;
            briefViewModel.LayoutAdapLimitations = brief.sLayOutAdapLimitations ;
            briefViewModel.InPrintBl = Convert.ToBoolean(brief.nInPrintBL ?? false);
            briefViewModel.CompetitionBrand4 = brief.sCompetitionBrand4 ;
            briefViewModel.InPainting = Convert.ToBoolean(brief.nInPainting ?? false);
            briefViewModel.TargetGroupBrand1 =brief.sTargetGroupBrand1 ;
            briefViewModel.TypeOfVehicles =brief.sTypofVehicles ;
            briefViewModel.TgsMedia1 = brief.sTGSMediaB1 ;
            briefViewModel.PreferredSize = brief.PreferredSize ;
            briefViewModel.OtherMediaVehicle = Convert.ToBoolean(brief.bOthers ?? false);
            briefViewModel.AdMix = brief.sAdMIX ;
            briefViewModel.DisplayType = brief.sDisplayType ;
            briefViewModel.TargetGroupBrand2 = brief.sTargetGroupBrand2 ;
            briefViewModel.TgsMedia2 = brief.sTGSMediaB2 ;
            briefViewModel.DisplayTypeSel =brief.sDisplayTypesSel ;
            briefViewModel.TgsMedia3 = brief.sTGSMediaB3;
            briefViewModel.TargetGroupBrand3 = brief.sTargetGroupBrand3 ;
            briefViewModel.TgsMedia4 = brief.sTGSMediaB4 ;
            briefViewModel.TargetGroupBrand4 = brief.sTargetGroupBrand4 ;
            briefViewModel.OutDoorObjective = brief.sOutdoorObjective ;
            briefViewModel.Market1 = brief.sMarket1 ;
            briefViewModel.Market2 = brief.sMarket2 ;
            briefViewModel.Market3 = brief.sMarket3 ;
            briefViewModel.Market4 = brief.sMarket4 ;
            briefViewModel.InVinyl = Convert.ToBoolean(brief.InVinyl ?? false);
            briefViewModel.Complete = Convert.ToBoolean(brief.Complete ?? false);
            briefViewModel.PartVinyl = Convert.ToBoolean(brief.PartVinyl ?? false);
            briefViewModel.Ratio11 = Convert.ToBoolean(brief.Ratio11 ?? false);
            briefViewModel.Ratio12 = Convert.ToBoolean(brief.Ratio12 ?? false);
            briefViewModel.Ratio13 = Convert.ToBoolean(brief.Ratio13 ?? false);
            briefViewModel.Ratio14 = Convert.ToBoolean(brief.Ratio14 ?? false);
            briefViewModel.Ratio34 = Convert.ToBoolean(brief.Ratio34 ?? false);
            briefViewModel.RatioAll = Convert.ToBoolean(brief.RatioAll ?? false);
            briefViewModel.BillBoard = Convert.ToBoolean(brief.BillBoard ?? false);
            briefViewModel.Buses = Convert.ToBoolean(brief.Buses ?? false); 
            briefViewModel.PoleKiosks = Convert.ToBoolean(brief.PoleKiosks?? false);
            briefViewModel.BusQShelter = Convert.ToBoolean(brief.BusQShelter?? false); 
            briefViewModel.TransitMedia = Convert.ToBoolean(brief.TransitMedia?? false);
            briefViewModel.PublicUtitities = Convert.ToBoolean(brief.PublicUtitities ?? false); ;
            briefViewModel.Others = brief.sOthers ;
            briefViewModel.IsInActive = Convert.ToBoolean(brief.IsInActive);
            return View(briefViewModel);
        }

        [HttpPost]
        public ActionResult Edit(BriefViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Edit", viewModel);

            var service = new MyNotesCommandServiceClient();
            var command = new ChangeBrief();
            command.BriefId = viewModel.BriefId;
            command.BriefDate = viewModel.BriefDate;
            command.Customer = viewModel.Customer;
            command.AdditionalInfo = viewModel.AdditionalInfo;
            command.BudgetValue = viewModel.BudgetValue;
            command.ContactPerson = command.ContactPersonId;
            command.ClientBusinessProfile = viewModel.ClientBusinessProfile;
            command.BudgetTypes = viewModel.BudgetTypes;
            command.IsApproved = viewModel.IsApproved;
            command.ApprovalNo = viewModel.ApprovalNo;
            command.ApprovalDate = viewModel.ApprovalDate;
            command.DeadLineDate = viewModel.DeadLineDate;
            command.Brand1 = viewModel.Brand1;
            command.TentativeStartDate = viewModel.TentativeStartDate;
            command.TentativeEndDate = viewModel.TentativeEndDate;
            command.NoOfYears = viewModel.NoOfYears;
            command.OtherStCommision = viewModel.OtherStCommision;
            command.MarketingObjective = viewModel.MarketingObjective;
            command.RequestSentToHO = viewModel.RequestSentToHO;
            command.CompetitionBrand1 = viewModel.CompetitionBrand1;
            command.CompetitionBrand2 = viewModel.CompetitionBrand2;
            command.PreparedBy = viewModel.PreparedBy;
            command.BriefNo = viewModel.BriefNo;
            command.Brand2 = viewModel.Brand2;
            command.NoOfMonths = viewModel.NoOfMonths;
            command.NoOfDays = viewModel.NoOfDays;
            command.BriefStatus = viewModel.BriefStatus;
            command.OsDays = viewModel.OsDays;
            command.DateCreated = viewModel.DateCreated;
            command.Brand3 = viewModel.Brand3;
            command.CompetitionBrand3 = viewModel.CompetitionBrand3;
            command.SendDate = viewModel.SendDate;
            command.HeadPlannerUserId = viewModel.HeadPlannerUserId;
            command.InPrintFl = viewModel.InPrintFl;
            command.AdObjective = viewModel.AdObjective;
            command.Brand4 = viewModel.Brand4;
            command.LayoutAdapLimitations = viewModel.LayoutAdapLimitations;
            command.InPrintBl = viewModel.InPrintBl;
            command.CompetitionBrand4 = viewModel.CompetitionBrand4;

            command.InPainting = viewModel.InPainting;
            command.TargetGroupBrand1 = viewModel.TargetGroupBrand1;
            command.TypeOfVehicles = viewModel.TypeOfVehicles;
            command.TgsMedia1 = viewModel.TgsMedia1;

            command.PreferredSize = viewModel.PreferredSize;
            command.Others = viewModel.Others;
            command.AdMix = viewModel.AdMix;
            command.DisplayType = viewModel.DisplayType;
            command.TargetGroupBrand2 = viewModel.TargetGroupBrand2;
            command.TgsMedia2 = viewModel.TgsMedia2;
            command.DisplayTypeSel = viewModel.DisplayTypeSel;
            command.TgsMedia3 = viewModel.TgsMedia3;
            command.TargetGroupBrand3 = viewModel.TargetGroupBrand3;
            command.TgsMedia4 = viewModel.TgsMedia4;
            command.TargetGroupBrand4 = viewModel.TargetGroupBrand4;
            command.OutDoorObjective = viewModel.OutDoorObjective;
            command.Market1 = viewModel.Market1;
            command.Market2 = viewModel.Market2;
            command.Market3 = viewModel.Market3;
            command.Market4 = viewModel.Market4;
            command.InVinyl = viewModel.InVinyl;
            command.Complete = viewModel.Complete;
            command.PartVinyl = viewModel.PartVinyl;
            command.Ratio11 = viewModel.Ratio11;
            command.Ratio12 = viewModel.Ratio12;
            command.Ratio13 = viewModel.Ratio13;
            command.Ratio14 = viewModel.Ratio14;
            command.Ratio34 = viewModel.Ratio34;
            command.RatioAll = viewModel.RatioAll;
            command.BillBoard = viewModel.BillBoard;
            command.Buses = viewModel.Buses;
            command.PoleKiosks = viewModel.PoleKiosks;
            command.BusQShelter = viewModel.BusQShelter;
            command.TransitMedia = viewModel.TransitMedia;
            command.PublicUtitities = viewModel.PublicUtitities;
            command.OtherMediaVehicle = viewModel.OtherMediaVehicle;
            command.IsInActive = viewModel.IsInActive;
            service.ChangeBrief(command);

           
            return RedirectToAction("Index");
        }
       
    }
}
