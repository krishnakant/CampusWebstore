using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
    public class OrderModel
    {
        public string OrderId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string ConfNumber { get; set; }

        public string DatePlaced { get; set; }

        public string Amount { get; set; }

        public string DateProcessed { get; set; }

        public string Status { get; set; }

    }
}
