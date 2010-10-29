using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDTOs
{
    public class PlanCityDTO
    {
        public Guid PlanCitiesId { get; set; }
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
        public string LocationId { get; set; }
    }
}
