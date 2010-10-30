using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    public class RegionAdded : SourcedEvent
    {
        public Guid PlanRegionId { get; set; }
        public Guid PlanDetailsId { get; set; }
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
    }

    [Serializable]
    public class RegionChanged : SourcedEvent
    {
        public Guid PlanRegionId { get; set; }
        public Guid PlanDetailsId { get; set; }
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
    }
}
