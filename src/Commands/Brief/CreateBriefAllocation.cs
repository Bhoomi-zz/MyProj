using System;
using System.Collections.Generic;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;
using CommonDTOs;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.BriefAllocation, Domain")]
    public class CreateBriefAllocation : CommandBase
    {
        public Guid PlanId { get; set; }
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
        public List<RegionsAndCitiesDTO> RegionCities { get; set; }
    }
}
