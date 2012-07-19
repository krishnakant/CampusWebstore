using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
    public class TaxModel
    {
        #region"Properties"

        public string Tax { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public string ErrorMsg { get; set; }

        #endregion

        public TaxModel()
        {
            PaymentMethods = new List<PaymentMethod>();
        }
    }

    public class PaymentMethod
    {
        #region"Properties"

        public string Type { get; set; }

        public string Tender { get; set; }

        #endregion

    }


}
