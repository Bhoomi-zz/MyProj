using System;
using System.Collections.Generic;

namespace CommonDTOs
{
    [Serializable]
    public class RegionsAndCitiesDTO 
    {
        public Guid RegionsAndCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string PlannerId { get; set; }
        public decimal Budget { get; set; }
    }
}
