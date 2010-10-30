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
     [MapsToAggregateRootMethod("Domain.Plan, Domain", "AddOrModifySiteInPlan")]
    public class CreateOrChangeSitesInPlan : CommandBase
    {
         [AggregateRootId]
        public Guid PlanDetailId { get; set; }
        public Dictionary<Guid, PlanSiteDTO> Sites { get; set; }
        public Dictionary<Guid, PlanSiteDTO> DeselectedSites { get; set; }
    }
}
