using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    internal class Photo
    {
        public Guid PhotoId { get; set; }
        public string PhotoName { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
    }
}
