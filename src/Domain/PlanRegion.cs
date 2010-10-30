using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class PlanRegion
    {
        public Guid PlanRegionId { get; set; }
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
    }
}
