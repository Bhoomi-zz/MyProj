using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class PlanSite
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
        public decimal DisplayClientRate { get; set; }
        public decimal DisplayClientCost { get; set; }

        public decimal SiteSizeInSqFt { get; set; }

        public string MountingVendor { get; set; }
        public decimal MountingRate { get; set; }
        public decimal MountingCost { get; set; }
        public decimal MountingClientRate { get; set; }
        public decimal MountingClientCost { get; set; }

        public string PrintingVendor { get; set; }
        public decimal PrintingRate { get; set; }
        public decimal PrintingCost { get; set; }
        public decimal PrintingClientRate { get; set; }
        public decimal PrintingClientCost { get; set; }

        public string FabricationVendor { get; set; }
        public decimal FabricationRate { get; set; }
        public decimal FabricationCost { get; set; }
        public decimal FabricationClientRate { get; set; }
        public decimal FabricationClientCost { get; set; }

        public string DisplayStatus { get; set; }
        public string MountingStatus { get; set; }
        public string PrintingStatus { get; set; }
        public string FabricationStatus { get; set; }
    }
}
