using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDTOs;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.PlanAlbum, Domain", "AddOrRemovePhotosFromPlanAlbum")]
    public class AddOrRemovePhotosFromPlanAlbum : CommandBase
    {
        [AggregateRootId]
        public Guid PlanAlbumId { get; set; }
        public IEnumerable<PhotoDetailDTO> AttachedPhotos { get; set; }
        public IEnumerable<PhotoDetailDTO> RemovedPhotos { get; set; }
    }
}
