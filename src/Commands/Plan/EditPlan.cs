using System;
using System.Collections.Generic;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;
using CommonDTOs;


namespace Commands
{
    [MapsToAggregateRootMethod("Domain.Plan, Domain", "ChangePlan")]
    public class EditPlan : CommandBase
    {
        [AggregateRootId]
        public Guid PlanDetailsId { get; set; }
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
        public int PlanNo { get; set; }
        public decimal Budget { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientId { get; set; }
    }
}
