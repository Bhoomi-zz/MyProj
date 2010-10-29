using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDTOs;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
   [MapsToAggregateRootConstructor("Domain.PlanAlbum, Domain")]
    public class CreatePlanAlbum : CommandBase
    {
       public Guid PlanDetailId { get; set; }
       public IEnumerable<PhotoDetailDTO> AttachedPhotos { get; set; }
    }
}
