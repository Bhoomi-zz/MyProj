using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Photo
{
    public class SiteAlbum
    {
        public Guid SiteId { get; set;}
        public string SiteName { get; set; }
        public int PhotoCount { get; set; }
        public Guid PlanCityId { get; set; }
        public Dictionary<Guid, PhotoViewModel> Photos { get; set; }
    }
}