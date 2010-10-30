using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;
namespace Events
{
    [Serializable]
    public class SiteMountingInfoChanged : SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string MountingVendor { get; set; }
        public decimal MountingRate { get; set; }
        public decimal MountingCost { get; set; }
        public decimal MountingClientRate { get; set; }
        public decimal MountingClientCost { get; set; }
        public string MountingStatus { get; set; }
    }
}
