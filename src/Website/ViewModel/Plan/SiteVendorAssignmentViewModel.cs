using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.ViewModel.Plan
{
    public class SiteVendorAssignmentViewModel
    {
        public Guid PlanDetailsId { get; set; }
        public string ClientName { get; set; }
        public string BriefNo { get; set; }
        public string BriefDate { get; set; }
        public int? PlanNo { get; set; }
        public int? PlanBudget { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Region { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string HeadPlannerName { get; set; }
        private string activity;
        public string Activity
        {
            get { return activity ?? ""; }
            set { activity = value; }
        }

        public IEnumerable<PlanRegionViewModel> Regions { get; set; }
        public IEnumerable<PlanCityViewModel> Cities { get; set; }

        private IEnumerable<string> _activityList { get { return new List<string>(){"Display", "Mounting", "Printing","Fabrication"};}}
        private string _activity { get; set; }

        private IEnumerable<string> _chargingBasis { get { return new List<string>() {"Per SqFt", "Per Site", "Lump sum"};} }
        public string ChargingBasis { get; set; }

        private IEnumerable<string> _statuses {get {  return new List<string>() {"Proposed", "Under Negotiation", "Booked"};} }
        public string Status { get; set; }
        private IEnumerable<PlanRegionViewModel> regionsWithAll
        {
            get
            {
                List<PlanRegionViewModel> rm = Regions.ToList();
                rm.Add(new PlanRegionViewModel() { Budget = 0, PlannerId = "", PlannerName = "", Region = "<All>" });
                return rm.OrderBy(x => x.Region);
            }
        }

        public SelectList RegionsList
        {
            get { return new SelectList(regionsWithAll, "Region", "Region", Region); }
        }

        private IEnumerable<PlanCityViewModel> citiesWithAll
        {
            get
            {
                List<PlanCityViewModel> rm = Cities.ToList();
                rm.Add(new PlanCityViewModel() { Budget = 0, PlannerId = "", PlannerName = "",LocationId = "<All>", LocationName = "<All>" });
                return rm.OrderBy(x => x.Region);
            }
        }

        public SelectList CitiesList
        {
            get { return new SelectList(citiesWithAll, "LocationIdWithRegion", "LocationNameWithSiteCount", CityId); }
        }
        public SelectList ActivityList
        {
            get { return new SelectList(_activityList); }
        }
        public SelectList ChargingBasisList
        {
            get { return new SelectList(_chargingBasis);}
        }
        public SelectList StatusList
        {
            get { return new SelectList(_statuses); }
        }
        public IEnumerable<PlanSite> PlanSites { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public int Rate { get; set; }
        public int ClientRate { get; set; }

    }
}