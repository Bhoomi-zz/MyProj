using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commands;
using CommonDTOs;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.Plan, Domain", "AddOrModifyCityInPlan")]
    public class CreateOrChangeCitiesInPlan : CommandBase
    {
        [AggregateRootId]
        public Guid PlanDetailId { get; set; }
        public Dictionary<Guid, PlanCityDTO> Cities { get; set; }
    }
}
