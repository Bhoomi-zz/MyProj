using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDTOs
{
    public class SiteVendorAssignmentDTO
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string Vendor { get; set; }
        public decimal Rate { get; set; }
        public decimal Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VendorType { get; set; }
        public string Status { get; set; }
        public decimal ClientRate { get; set; }
        public decimal ClientCost { get; set; }
        public string ChargingBasis { get; set; }
     }
}
