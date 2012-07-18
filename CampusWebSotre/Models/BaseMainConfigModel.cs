using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusWebStore.Models
{
    public class BaseMainConfigModel
    {
        public bool DisplayPrices { get; set; }
        public bool WholeSaleBooks { get; set; }
        public bool RetailBooks { get; set; }
        public bool ShowBuyBackNeed { get; set; }
        public bool StoreCredit { get; set; }
        public bool Vouchers { get; set; }
        public string VoucherExpire { get; set; }

        public bool RequireLogin { get; set; }

        public string Disclaimer { get; set; }

        public string StoreCreditPercent { get; set; }
        public string RetailPercent { get; set; }
        public string WholeSalePercent { get; set; }
        public string RetailCoin { get; set; }
        public string RetailRounding { get; set; }
        public string WholeSaleCoin { get; set; }
        public string WholeSaleRounding { get; set; }

    }
}