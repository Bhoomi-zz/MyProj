using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using Website.Services;
using System.ComponentModel.DataAnnotations;
namespace Website.ViewModel.Plan
{
    public class PlanCreateViewModel 
    {
        
        public Guid PlanDetailsId { get; set; }

        [Required(ErrorMessage = "Brief No is Required")]
        [Range(1, 100000, ErrorMessage = "Please enter a Brief No")]
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
        public int PlanNo { get; set; }
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "StartDate is Required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is Required")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Client is Required")]
        public string ClientId { get; set; }
        public SelectList Users
        {
            get { return new SelectList(SharedDataService.GetAllUsers(), "sUserId", "sUserName", HeadPlannerId); }
        }
        [Required(ErrorMessage = "Client is Required")]
        public string ClientName { get; set; }
    }
}