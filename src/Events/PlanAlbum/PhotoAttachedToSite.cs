using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events.PlanAlbum
{
    [Serializable]
    public class PhotoAttachedToSite: SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public Guid PhotoId { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string PhotoName { get; set; } 
    }
}
