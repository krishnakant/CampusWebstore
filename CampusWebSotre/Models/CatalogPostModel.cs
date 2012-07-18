using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Models
{
    public class CatalogPostModel
    {
        #region Properties
        [Required]
        public string Description { get; set; }
         [Required]
        public string StartDate { get; set; }
         [Required]
        public string EndDate { get; set; }
       
        public bool DisplayTumbnail { get; set; }
         
        public string Comment { get; set; }

        public string Webcolumn { get; set; }

        public string SortSquence { get; set; }
        public string Key { get; set; }

        public IEnumerable<AdminCatalogItemViewModel> AdminCatalogItemViewModel { get; set; }

        #endregion

    }
}
