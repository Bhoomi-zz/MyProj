using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using CommonDTOs;
using ReadModel;
using Website.Services;
using System.ComponentModel.DataAnnotations;

namespace Website.ViewModel
{
    public class BriefAllocationViewModelForEdit 
    {
         [Required(ErrorMessage = "Plan Id is Required")]
        public Guid PlanId { get; set; }
         [Required(ErrorMessage = "Brief No is Required")]
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
        public List<RegionsAndCitiesDTO> RegionCities { get; set; }
        public List<RegionsAndCitiesViewModel> RegionAndCities;
        public object UserList { get { return Users; } }

        public BriefAllocationViewModelForEdit()
        {
            RegionAndCities = new List<RegionsAndCitiesViewModel>();
        }
        public  SelectList Users
        {
            get { return new SelectList(SharedDataService.GetAllUsers(), "sUserId", "sUserName", HeadPlannerId); }
        }
    }
}