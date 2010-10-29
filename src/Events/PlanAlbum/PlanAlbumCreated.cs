using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events.PlanAlbum
{
    [Serializable]
    public class PlanAlbumCreated : SourcedEvent
    {
        public Guid PlanAlbumId;
        public Guid PlanDetailsId;
    }
}
