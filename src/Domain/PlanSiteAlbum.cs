using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    internal class PlanSiteAlbum
    {
        public Guid PlanSiteId { get; set; }
        public Dictionary<Guid, Photo> AttachedPhotos { get; set; }    
    }
}
