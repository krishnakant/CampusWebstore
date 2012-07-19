using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
    public class SellBackBookModel
    {
        #region Properties

        public string Isbn { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Edition { get; set; }
        public string Copyright { get; set; }
        public string WhslrPrice { get; set; }
        public string RetStoreCredit { get; set; }
        public string WhsleStoreCredit { get; set; }
        public string NeedLimit { get; set; }
        public string NeedWording { get; set; }
        public string RetailWording { get; set; }
        public string WholeSaleWording { get; set; }
        public string BuyingWording { get; set; }
        public string RetailPrice { get; set; }
        public string NotBuyingWording { get; set; }
        public string ErrMsg { get; set; }

        #endregion
    }
}
