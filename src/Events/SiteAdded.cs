using System;
using System.Collections.Generic;
using Ncqrs.Eventing.Sourcing;


namespace Events
{
    [Serializable]
    public class SiteAdded : SourcedEvent
    {
        public Guid PlanSiteId { get; set; }
        public Guid PlanCityId { get; set; }
        public int SiteNo { get; set; }
        public string SiteName { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string SiteSize { get; set; }
        public string Illumination { get; set; }
        public int H { get; set; }
        public int V { get; set; }
        public string SizeRatio { get; set; }
        public string MediaType { get; set; }
        public int NoOfFaces { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Days { get; set; }
        public int Qty { get; set; }
        public string DisplayVendor { get; set; }
        public decimal DisplayRate { get; set; }
        public decimal DisplayCost { get; set; }
        public DateTime? DisplayFromDate { get; set; }
        public DateTime? DisplayToDate { get; set; }
        public decimal SiteSizeInSqFt { get; set; }
    }
}
