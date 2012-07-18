using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusWebStore.Models
{
    public class ElementResponseModel
    {
        public string TransactionSetupId { get; set; }
        public string ValidationCode { get; set; }

        public string GetTransactionURL()
        {
            if (TransactionSetupId != null) {
                return "https://certtransaction.hostedpayments.com?TransactionSetupid=" + TransactionSetupId;
            }
            else {
                return null;
            }
        }
    }
}