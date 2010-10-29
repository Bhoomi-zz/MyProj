using System;
using System.ComponentModel;
using System.Web.Mvc;
using Commands;
using CommonDTOs;
namespace Website.ViewModel
{
    public class RegionsAndCitiesViewModel : RegionsAndCitiesDTO
    {
        public Guid BriefAllocationId { get; set; }
        public string LocationName { get; set; }
        [DisplayName("PlannerName")]
        public string PlannerName { get; set; }
    }
}