using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Photo
{
    public class PhotoViewModel
    {
        public Guid PlanAlbumId { get; set; }
        public Guid PhotoId { get; set; }
        public string PhotoName { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
    }
}