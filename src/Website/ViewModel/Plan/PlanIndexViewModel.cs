using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadModel;

namespace Website.ViewModel
{
    public class PlanIndexViewModel : PlanDetail
    {
        public string HeadPlanner { get; set; }
        public string ClientName { get; set; }
    }
}