using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using ReadModel;
using Website.Services;

namespace Website.ViewModel
{
    public class BriefAllocationViewModelForIndex
    {
        public Guid BriefAllocationId { get; set; }
        public string BriefNo { get; set; }
        public string HeadPlanner { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string contactId { get; set; }
        public string ClientName { get; set; }
    }
}