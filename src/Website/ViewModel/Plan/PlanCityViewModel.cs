using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Plan
{
    public class PlanCityViewModel
    {
        public Guid PlanCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationNameWithSiteCount
        {
            get
            {
                return LocationName=="<All>" ? LocationName : LocationName + " (" + SiteCount + ")";
            }
        }
        public string LocationIdWithRegion {
            get
            {
                return LocationId + ";" + Region;
            }
        }
        public string PlannerId { get; set; }
        public string PlannerName { get; set; }
        public decimal Budget { get; set; }
        public int SiteCount { get; set; }
        public bool IsDirty { get; set; }
    }
}