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
    [MapsToAggregateRootMethod("Domain.Plan, Domain", "AddOrModifyRegionInPlan")]
    public class CreateRegionInPlan : CommandBase
    {
        [AggregateRootId]
        public Guid PlanDetailId { get; set; }
        public Dictionary<Guid, PlanRegionsDTO> Regions { get; set; }
    }
}
