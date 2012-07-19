using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
   public class AdminCatalogItemModel
    {
        #region Properties

        public string Sku { get; set; }

        public string Mod { get; set; }

        public string Title { get; set; }

        public string Price { get; set; }

        public string SalesPrice { get; set; }

        #endregion
    }
}
