using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using ReadModel;
using Website.Services;
using System.ComponentModel.DataAnnotations;
namespace Website.ViewModel
{
    public class EditBriefViewModel : ChangeBrief
    {
         //private readonly Services.SharedDataService _sharedDataService ;
         public EditBriefViewModel()
        {
           // _sharedDataService = new SharedDataService();
        }
        
         public  string Customerq { get; set; }
    

        public SelectList Customers
        {
            get { return new SelectList(SharedDataService.GetAllCustomers(), "sContactId", "sName", Customer); }
        }
        public SelectList BudgetTypesList
        {
            get
            {
                var budgetTypes = new List<string> { "Display", "Display + Production", "All Inclusive" };
                return new SelectList(budgetTypes);
            }
        }
        public SelectList YesNoList
        {
            get
            {
                return  new SelectList( new List<string>{"Yes", "No"});
            }
        }
        public string GetContactPersonName(string contactPersonId)
        {
            return SharedDataService.GetContactPersonNameById(contactPersonId);
        }
    }
}