using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.ViewModel.Plan;

namespace Website.ViewModel.Photo
{
    public class PlanPhotoUploadingViewModel
    {
        public Guid planDetailsId { get; set; }
        public Guid PlanAlbumId { get; set; }
        public int PlanNo { get; set; }
        public string ClientName { get; set; }
        public string Region { get; set; }
        public string SelectedCityId { get; set; }
        public IEnumerable<PlanRegionViewModel> Regions { get; set; }
        public Guid PhotoAgId { get; set; }
        public int AlbumNo { get; set; }
        public int sessionId { get; set; }
        public SelectList RegionsList
        {
            get
            {
                return new SelectList(Regions, "Region", "Region", Region);
            }
        }
        public List<PlanCityforPhotosViewModel> Cities { get; set; }
        public IEnumerable<PhotoViewModel> UnAttachedPhotos { get; set; }
     }
}