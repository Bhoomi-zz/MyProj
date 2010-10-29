using System;
using Ncqrs;
using Events;
using Ncqrs.Domain;

namespace Domain
{
    public class RegionsAndCities
    {
        public Guid RegionAndCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string PlannerId { get; set; }
        public decimal Budget { get; set; }
    }
}
