using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.ViewModel.Plan;

namespace Website.ViewModel.Photo
{
    public class PlanSiteAlbumViewModel
    {
        public Guid PlanId { get; set; }
        public Guid PlanAlbumId { get; set; }
        public int PlanNo { get; set;}
        public string ClientName { get; set;}
        public string Region { get; set; }
        public Guid PlanCityId { get; set; }
        
        public string CityName { get; set; }
        public string SelectedSiteId { get; set; } //Used after user has submitted the record.
        public IEnumerable<PlanRegionViewModel> Regions { get; set; }
        public SelectList RegionsList
        {
            get
            {
                return new SelectList(Regions, "Region", "RegionNameWithSiteCount", Region);
            }
        }
        public IEnumerable<PlanCityforPhotosViewModel> Cities { get; set; }
        public SelectList CitiesList
        {
            get
            {
                return new SelectList(Cities, "LocationIdWithRegion" , "LocationNameWithSiteCount", PlanCityId);
            }
        }
        public List<SiteAlbum> Sites { get; set; }
        public Dictionary<Guid, PhotoViewModel> UnAttachedPhotos { get; set; }

        
    }
}