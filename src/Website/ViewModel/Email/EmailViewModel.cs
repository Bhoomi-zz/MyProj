using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.ViewModel.Email
{
    public class EmailViewModel
    {
        public Guid PlanEmailId { get; set; }
        public Guid PlanDetailsId { get; set; }
        public int PlanNo { get; set; }
        public string Client { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set;}
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
        public string SentBy { get; set; }
        public bool HasSucceeded { get; set; }
        public string FileName { get; set; }
        public IEnumerable<EmailDataViewModel> EmailDataViewModels { get; set; }
    }
}