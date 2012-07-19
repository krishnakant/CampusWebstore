using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
  public  class OrderDetail
    {
        #region "Order Header"

        public string UserId { get; set; }

        public string OrderId { get; set; }

        public string OrderDate { get; set; }

        public string Status { get; set; }

        public string StatusDate { get; set; }

        #endregion

        #region "Billing"

        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        #endregion

        #region "Shipping"

        public string ShipToName { get; set; }

        public string ShipAddress1 { get; set; }

        public string ShipAddress2 { get; set; }

        public string ShipCity { get; set; }

        public string ShipState { get; set; }

        public string ShipZip { get; set; }

        public string ShipCountry { get; set; }

        #endregion

        #region "Order Detail"

        public string ShipMethod { get; set; }

        public string SubTotal { get; set; }

        public string Freight { get; set; }

        public string Total { get; set; }

        #endregion
        #region "Info Detail"

        public string FreightType { get; set; }

        public string PromoCode { get; set; }

        public string GiftCard { get; set; }

        #endregion

        #region "Item Detail"

        public string Title { get; set; }

        public string EbookUrl { get; set; }

        public string EbookInfo { get; set; }

        public string Author { get; set; }

        public string Memo { get; set; }

        public string Qty { get; set; }

        public string Price { get; set; }

        public string ExtPrice { get; set; }

        public string Type { get; set; }

        #endregion

        #region "Error Message"

        public string ErrorMsg { get; set; }

        #endregion
    }
}
