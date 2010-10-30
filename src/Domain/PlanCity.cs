using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class PlanCity
    {
        public Guid PlanCityId { get; set; }
       
        public string Region { get; set; }
        public decimal Budget { get; set; }
        public string PlannerId { get; set; }
        public string LocationId { get; set; }
        
        private Dictionary<Guid, PlanSite> _sites;
        public Dictionary<Guid, PlanSite> Sites
        {
            get { return _sites ?? (_sites = new Dictionary<Guid, PlanSite>()); }
            set { _sites = value; }
        }
        private Dictionary<Guid, PlanSite> _deselectedsites;
        public Dictionary<Guid, PlanSite> DeselectedSites
        {
            get { return _deselectedsites ?? (_deselectedsites = new Dictionary<Guid, PlanSite>()); }
            set { _deselectedsites = value; }
        }
    }
}
