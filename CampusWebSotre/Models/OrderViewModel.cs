using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusWebStore.Models
{
    public class OrderViewModel
    {
        [Display(Name = "OrderID")]
        public string OrderId { get; set; }

        [Display(Name = "Customer ID")]
        public string UserId { get; set; }

        [Display(Name = "Customer Name")]
        public string UserName { get; set; }

        [Display(Name = "Conf #")]
        public string ConfNumber { get; set; }

        [Display(Name = "Date Placed")]
        public string DatePlaced { get; set; }
        
        [Display(Name = "Amount")]
        public string Amount { get; set; }

        [Display(Name = "Date Processed")]
        public string DateProcessed { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}