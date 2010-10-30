using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using ReadModel;
using Website.Services;
using System.ComponentModel.DataAnnotations;

namespace Website.ViewModel
{
    public class BriefViewModel
    {
        //public IEnumerable<bmContact> Customers { get; set; }
        //private readonly Services.SharedDataService _sharedDataService ;

        public BriefViewModel()
        {
          //  _sharedDataService = new SharedDataService();
        }
        public Guid BriefId { get; set; }
        [Required(ErrorMessage = "Brief No is Required")]
        public DateTime? BriefDate { get; set; }
         [Required(ErrorMessage = "Customer is Required")]
        public string Customer { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal BudgetValue { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonId { get; set; }
        public string ClientBusinessProfile { get; set; }
        public string BudgetTypes { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovalNo { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DeadLineDate { get; set; }
        public string Brand1 { get; set; }
        public DateTime? TentativeStartDate { get; set; }
        public DateTime? TentativeEndDate { get; set; }
        public int NoOfYears { get; set; }
        public int OtherStCommision { get; set; }
        public string MarketingObjective { get; set; }
        public DateTime? RequestSentToHO { get; set; }
        public string CompetitionBrand1 { get; set; }
        public string CompetitionBrand2 { get; set; }
        public string PreparedBy { get; set; }
        public int BriefNo { get; set; }
        public string Brand2 { get; set; }
        public int NoOfMonths { get; set; }
        public int NoOfDays { get; set; }
        public int BriefStatus { get; set; }
        public int OsDays { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Brand3 { get; set; }
        public string CompetitionBrand3 { get; set; }
        public DateTime? SendDate { get; set; }
        public Guid HeadPlannerUserId { get; set; }
        public bool InPrintFl { get; set; }
        public string AdObjective { get; set; }
        public string Brand4 { get; set; }
        public string LayoutAdapLimitations { get; set; }
        public bool InPrintBl { get; set; }
        public string CompetitionBrand4 { get; set; }

        public bool InPainting { get; set; }
        public string TargetGroupBrand1 { get; set; }
        public string TypeOfVehicles { get; set; }
        public string TgsMedia1 { get; set; }

        public string PreferredSize { get; set; }
        public string Others { get; set; }
        public string AdMix { get; set; }
        public string DisplayType { get; set; }
        public string TargetGroupBrand2 { get; set; }
        public string TgsMedia2 { get; set; }
        public string DisplayTypeSel { get; set; }
        public string TgsMedia3 { get; set; }
        public string TargetGroupBrand3 { get; set; }
        public string TgsMedia4 { get; set; }
        public string TargetGroupBrand4 { get; set; }
        public string OutDoorObjective { get; set; }
        public string Market1 { get; set; }
        public string Market2 { get; set; }
        public string Market3 { get; set; }
        public string Market4 { get; set; }
        public bool InVinyl { get; set; }
        public bool Complete { get; set; }
        public bool PartVinyl { get; set; }
        public bool Ratio11 { get; set; }
        public bool Ratio12 { get; set; }
        public bool Ratio13 { get; set; }
        public bool Ratio14 { get; set; }
        public bool Ratio34 { get; set; }
        public bool RatioAll { get; set; }
        public bool BillBoard { get; set; }
        public bool Buses { get; set; }
        public bool PoleKiosks { get; set; }
        public bool BusQShelter { get; set; }
        public bool TransitMedia { get; set; }
        public bool PublicUtitities { get; set; }
        public bool OtherMediaVehicle { get; set; }
        public bool IsInActive { get; set; }
        public SelectList Customers
        {
            get { return new SelectList(SharedDataService.GetAllCustomers(), "sContactId", "sName", Customer); }
        }
        public SelectList BudgetTypesList
        {
            get
            {
                var budgetTypes = new List<string> { "Display", "Display + Production", "All Inclusive" };
                return new SelectList(budgetTypes);
            }
        }
        public SelectList YesNoList
        {
            get
            {
                return  new SelectList( new List<string> {"Yes", "No"});
            }
        }
        public string GetContactPersonName(string contactPersonId)
        {
            return SharedDataService.GetContactPersonNameById(contactPersonId);
        }
    }
}