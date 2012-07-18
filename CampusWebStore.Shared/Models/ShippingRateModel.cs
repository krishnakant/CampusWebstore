using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
    public class ShippingRateModel
    {
        #region "Properties"

        public List<ShippingMethod> ShippingMethods { get; set; }

        public string ConfNumber { get; set; }

        public string ErrorMsg { get; set; }

        #endregion


        public ShippingRateModel()
        {
            ShippingMethods = new List<ShippingMethod>();
        }
    }

    public class ShippingMethod
    {

        #region "Properties"

        public string Display { get; set; }

        public string Amount { get; set; }

        public string Table { get; set; }

        #endregion 
    }
}
