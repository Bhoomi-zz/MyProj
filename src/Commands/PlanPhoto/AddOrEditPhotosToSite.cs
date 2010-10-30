using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDTOs;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.PlanSitePhotos, Domain", "AddOrEditPlanSitePhotos")]
    public class AddOrEditPhotosToSite: CommandBase
    {
        [AggregateRootId]
        public Guid AgId { get; set; }
        public Guid PlanCityId { get; set; }
        public List<PhotoDetailDTO> AttachedPhotoDetailDtos { get; set;}
        public List<PhotoDetailDTO> UnAttachedPhotoDetailDtos { get; set; }
    }
}
