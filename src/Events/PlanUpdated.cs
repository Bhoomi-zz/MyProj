using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    public class PlanUpdated : SourcedEvent
    {
        public Guid PlanDetailsId { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
