using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using CommonDTOs;
using ReadModel;
using Website.Services;

namespace Website.ViewModel
{

    public class BriefAllocationViewModelForCreate 
    {
        public Guid PlanId { get; set; }
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
      
        public List<RegionsAndCitiesDTO> RegionCities { get; set; }
        public List<RegionsAndCitiesViewModel> RegionAndCities;
        public object UserList { get { return Users; } }
            
        public BriefAllocationViewModelForCreate()
        {
            //_sharedDataService = new SharedDataService();
            RegionAndCities = new List<RegionsAndCitiesViewModel>();
        }
        public  SelectList Users
        {
            get { return new SelectList(SharedDataService.GetAllUsers(), "sUserId", "sUserName", HeadPlannerId); }
        }
    }
}