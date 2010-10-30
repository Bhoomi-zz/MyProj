using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.ViewModel.Plan
{
    public class PlanSiteViewModel
    {
        public Guid PlanDetailsId { get; set; }
        public string BriefNo { get; set; }
        public int PlanNo { get; set; }
        public decimal Budget { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Region { get; set; }
       
        private string _regionString;
        public string RegionString
        {
            get
            {
                // selectedRegions = "{ value: \"North:North;South:South;East:East;West:West\" }";
                //_regionString="{ value: \"";
                if (Regions != null)
                {
                    foreach (var planRegionViewModel in Regions)
                    {
                        _regionString += planRegionViewModel.Region + ":" + planRegionViewModel.Region + ";";
                    }

                    if (!string.IsNullOrEmpty(_regionString))
                        _regionString = _regionString.Substring(0, _regionString.Length - 1);
                }
                // _regionString+= "\" }";
                return _regionString;
            }
            set { _regionString = value; }
        }
        public decimal RegionBudget { get; set; }
        public string PlannerId { get; set; }
        public string PlannerName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string CityBudget { get; set; }
        public string CityBudgetH { get; set; }
        public Guid PlanCityId { get; set; }
        
        public IEnumerable<PlanRegionViewModel> Regions { get; set; }

        public SelectList RegionsList
        {
            get
            {
                return new SelectList(Regions, "Region", "RegionNameWithSiteCount", Region);
            }
        }

        public IEnumerable<PlanCityViewModel> PlanCities { get; set; }
        public SelectList CitiesList
        {
            get
            {
                return new SelectList(PlanCities, "LocationIdWithRegion", "LocationNameWithSiteCount", LocationName);
            }
        }

        public IEnumerable<PlanSite> PlanSites { get; set; }

        public List<int> DeselectedSites { get; set; }
    }
}