using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    public class SiteDisplayInfoChanged : SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string DisplayVendor { get; set; }
        public decimal DisplayRate { get; set; }
        public decimal DisplayCost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal DisplayClientRate { get; set; }
        public decimal DisplayClientCost { get; set; }
        public string DisplayStatus { get; set; }
    }
}
