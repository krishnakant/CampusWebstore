using System.Collections.Generic;
using CampusWebStore.Shared.Models;

namespace CampusWebSotre.Models
{
    public class AdminCatalogItemViewModel
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
