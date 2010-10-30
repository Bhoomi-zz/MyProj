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
     [MapsToAggregateRootMethod("Domain.Plan, Domain", "ChangeMountingInfo")]
    public class ChangeMountingInfoForSite : CommandBase
    {
        [AggregateRootId]
        public Guid PlanDetailsId { get; set; }
        public Guid PlanCityId { get; set; }
        public Dictionary<Guid, PlanSiteMountingInfoDTO> Sites { get; set; }
    }
}
