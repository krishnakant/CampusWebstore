using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CampusWebStore.Shared.Models;

namespace CampusWebSotre.Models
{
    public class CatalogItemModel
    {
        #region Properties
        [Required]
        public string Sku { get; set; }
         [Required]
        public string Title { get; set; }
         [Required]
        public string Source { get; set; }
       
        public bool Price { get; set; }
         
        public string SalesPrice { get; set; }
        
        #endregion

    }
}
