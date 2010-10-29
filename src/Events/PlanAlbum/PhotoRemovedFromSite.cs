using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events.PlanAlbum
{
    [Serializable]
    public class PhotoRemovedFromSite : SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
