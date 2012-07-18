using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Models
{
    public class PrintVoucherViewModel
    {
        public UserModel UserModel { get; set; }

        public IEnumerable<SellBackCartViewModel> SellBackModel { get; set; }

    }
}