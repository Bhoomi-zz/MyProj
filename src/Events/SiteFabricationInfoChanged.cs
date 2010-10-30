using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    public class SiteFabricationInfoChanged : SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string FabricationVendor { get; set; }
        public decimal FabricationRate { get; set; }
        public decimal FabricationCost { get; set; }
        public string FabricationStatus { get; set; }
        public decimal FabricationClientRate { get; set; }
        public decimal FabricationClientCost { get; set; }
    }
}
