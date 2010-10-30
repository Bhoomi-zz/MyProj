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
    [MapsToAggregateRootMethod("Domain.Plan, Domain", "ChangeDisplayInfo")]
    public class ChangeDisplayInfoForSite  : CommandBase
    {
        [AggregateRootId]
        public Guid PlanDetailsId { get; set; }
        public Guid PlanCityId { get; set; }
        public Dictionary<Guid, PlanSiteDisplayInfoDTO> Sites { get; set; }
    }
}
