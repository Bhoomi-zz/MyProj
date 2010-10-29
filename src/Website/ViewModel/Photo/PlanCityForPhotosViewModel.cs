using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Photo
{
    public class PlanCityforPhotosViewModel
    {
        public Guid PlanCitiesId { get; set; }
        public string Region { get; set; }
        public Guid PhotoAgId { get; set; }
        public string LocationName { get; set; }
        public string LocationIdWithRegion
        {
            get
            {
                return PlanCitiesId + ";" + Region;
            }
        }
        public string LocationNameWithSiteCount
        {
            get
            {
                return LocationName == "<All>" ? LocationName : LocationName + " (" + SiteCount + ")";
            }
        }
        public int SiteCount { get; set; }

        
    }
}