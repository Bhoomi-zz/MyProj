using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events.PlanAlbum
{
    [Serializable]
    public class PhotoRemovedFromPlanAlbum:SourcedEvent
    {
        public Guid PlanAlbumId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
