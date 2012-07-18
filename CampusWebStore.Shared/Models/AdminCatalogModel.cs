using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
   public class AdminCatalogModel
    {
        #region Properties
      
        public string Description { get; set; }
    
        public string StartDate { get; set; }
       
        public string EndDate { get; set; }

        public bool DisplayTumbnail { get; set; }

        public string Comment { get; set; }

        public string Webcolumn { get; set; }

        public string SortSquence { get; set; }

        public string Key { get; set; }

        public IEnumerable<AdminCatalogItemModel> AdminCatalogItemModels { get; set; }


        #endregion
    }
}
