using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDTOs;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.PlanSitePhotos, Domain")]
    public class AddPhotosToSite : CommandBase
    {
         public  Guid AgId { get; set; }
         public Guid PlanId { get; set; }
         public Guid PlanCityId { get; set; }
         public List<PhotoDetailDTO> AttachedPhotoDetailDtos { get; set;}
    }
}
