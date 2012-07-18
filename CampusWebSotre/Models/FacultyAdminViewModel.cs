using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Models
{ 

    public class FacultyAdminViewModel
    {
        #region Properties

        [Display(Name = "Faculty Admin Email")]
        [DataType(DataType.EmailAddress)]
        public string FacultyAdminEmail {get;set;}
        
        [Display(Name = "Faculty Secondary Email")]
        [DataType(DataType.EmailAddress)]
        public string FacultySecondaryEmail {get;set;}
        
        #endregion

    }
}