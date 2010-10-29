using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ncqrs.Eventing.Sourcing;
namespace Events
{
    [Serializable]
    public class CityAdded : SourcedEvent
    {
        public Guid PlanCitiesId { get; set; }
        public Guid PlanDetailsId { get; set; }
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
        public string LocationId { get; set; }
    }

    [Serializable]
    public class CityChanged : SourcedEvent
    {
        public Guid PlanCitiesId { get; set; }
        public Guid PlanDetailsId { get; set; }
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
        public string LocationId { get; set; }
    }
}
