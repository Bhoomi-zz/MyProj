using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    internal class PlanCityAlbum
    {
        public Guid PlanCityId { get; set; }
        public Dictionary<Guid, Photo> AttachedPhotos { get; set; }
        public Dictionary<Guid, PlanSiteAlbum> SiteAlbum { get; set; }
    }
}
