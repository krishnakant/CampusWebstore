using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CampusWebStore.Models
{
    public class UConnViewModel
    {
        #region "Properties"

        [Display(Name = "Yes, activate my membership!")]
        public string MemberID { get; set; }

        [Display(Name = " I do not wish to receive emails")]
        public bool EmailList { get; set; }

        [Display(Name = "Check Activation")]
        public string MemberIdCheck { get; set; }

        #endregion
    }
}