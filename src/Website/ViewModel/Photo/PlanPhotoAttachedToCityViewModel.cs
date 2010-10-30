using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Photo
{
    public class PlanPhotoAttachedToCityViewModel
    {
        public Guid PlanCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationNameWithSiteCount
        {
            get
            {
                return LocationName == "<All>" ? LocationName : LocationName + " (" + SiteCount + ")";
            }
        }
        public string LocationIdWithRegion
        {
            get
            {
                return LocationId + ";" + Region;
            }
        }
        public int SiteCount { get; set; }
        public Dictionary<Guid, PhotoViewModel> AttachedPhotos { get; set; }
    }
}