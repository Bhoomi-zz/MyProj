using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Photo
{
    public class PlanAlbumViewModel
    {
        public Guid PlanAlbumId { get; set; }
        public Guid PlanDetailsId { get; set; }
        public int PlanNo { get; set; }
        public string ClientName { get; set; }
        public string AlbumNo { get; set; } 
    }
}