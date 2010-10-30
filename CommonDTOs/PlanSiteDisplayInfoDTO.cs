using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDTOs
{
    public class PlanSiteDisplayInfoDTO
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public string DisplayVendor { get; set; }
        public decimal DisplayRate { get; set; }
        public decimal DisplayCost { get; set; }
        public DateTime? DisplayFromDate { get; set; }
        public DateTime? DisplayToDate { get; set; }
        public decimal DisplayClientRate { get; set; }
        public decimal DisplayClientCost { get; set; }
    }
}
