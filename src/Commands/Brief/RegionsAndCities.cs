using System;
using System.Collections.Generic;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [Serializable]
    public class RegionsAndCities1 : CommandBase
    {
        public Guid AllocatedRegionAndCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string PlannerId { get; set; }
        public decimal Budget { get; set; }
    }
}
