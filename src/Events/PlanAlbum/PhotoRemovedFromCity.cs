using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events.PlanAlbum
{
    public class PhotoRemovedFromCity : SourcedEvent
    {
        public Guid PlanCityId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
