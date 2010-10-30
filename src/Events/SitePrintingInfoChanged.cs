using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
     [Serializable]
    public class SitePrintingInfoChanged : SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string PrintingVendor { get; set; }
        public decimal PrintingRate { get; set; }
        public decimal PrintingCost { get; set; }
        public string PrintingStatus { get; set; }
        public decimal PrintingClientRate { get; set; }
        public decimal PrintingClientCost { get; set; }
    }
}
