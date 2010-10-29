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
    public class BriefViewModelIndex 
    {
        public  string BriefId { get; set; }
        public string Customer { get; set; }
        public DateTime BriefDate { get; set; }
        public string BriefNo { get; set; }
        public BriefViewModelIndex()
        {

        }
    }
}