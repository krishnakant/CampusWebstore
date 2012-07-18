using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CampusWebStore.Models
{
    public class CalendarViewModel
    {
        #region "Properties"
       [Display(Name = "ID:")]
       [Required]
        public string CalandarId { get; set; }

       [Display(Name = "Title:")]
       [Required]
        public string Title { get; set; }

       [Display(Name = "MeetingDate:")]
       [Required]
        public DateTime MeetingDate { get; set; }

       [Display(Name = "StartTime:")]
        public string StartTime { get; set; }

       [Display(Name = "EndTime:")]
        public string EndTime { get; set; }

        [Display(Name = "Location:")]
        public string Location { get; set; }

        #endregion
    }
}