using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Photo
{
    public class PlanAlbumIndexViewModel
    {
        public Guid PlanDetailsId { get; set;}
        public Guid PlanAlbumId { get; set;}
        public int PlanNo { get; set;}
        public string ClientName { get; set; }
    }
}