using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CampusWebStore.Models
{
    public class AddLinkViewModel
    {
        #region "Properties"

        [Display(Name = "Link Order:")]
        [Required]
        public string LinkOrder { get; set; }

        [Display(Name = "Link Title:")]
        [Required]
        public string LinkTitle { get; set; }

        [Display(Name = "Link Address:")]
        [Required]
        public string LinkAddress { get; set; }
       
        #endregion

    }
}
