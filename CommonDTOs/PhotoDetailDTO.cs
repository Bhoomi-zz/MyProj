using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDTOs
{
    public class PhotoDetailDTO
    {
        public Guid PhotoId { get; set; }
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
    }
}
