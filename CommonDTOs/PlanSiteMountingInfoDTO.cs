using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDTOs
{
    public class PlanSiteMountingInfoDTO
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string MountingVendor { get; set; }
        public decimal MountingRate { get; set; }
        public decimal MountingCost { get; set; }
        public decimal MountingClientRate { get; set; }
        public decimal MountingClientCost { get; set; }
    }
}
