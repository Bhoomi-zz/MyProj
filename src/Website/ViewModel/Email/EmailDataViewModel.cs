using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Email
{
    public class EmailDataViewModel
    {
        public int planno { get; set; }
        public string sUserName { get; set; }
        public decimal budget { get; set; }
        public string sname { get; set; }
        public string sLocationName { get; set; }
        public int siteNo { get; set; }
        public string siteName { get; set; }
        public string siteSize { get; set; }
        public string Illumination { get; set; }
        public int H { get; set; }
        public int V { get; set; }
        public string SizeRatio { get; set; }
        public string MediaType { get; set; }
        public int NoOfFaces { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public int Qty { get; set; }
        public decimal Cost { get; set; }
    }
}