using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
    public class CatalogItemModel
    {
        #region Properties

        public string Sku { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public string Comments { get; set; }

        public string Images { get; set; }

        public string Price { get; set; }

        public string CatalogPrice { get; set; }

        public string Desc { get; set; }

        public string CatId { get; set; }

        public string SeoTitle { get; set; }

        public string SeoSku { get; set; }

        public string RealPrice { get; set; }

        public string DisplayText { get; set; }

        public string Author { get; set; }

        public string SeoUrl { get; set; }

        public IEnumerable<ItemLookUpModel> ItemLookUpModel { get; set; }

        #endregion

    }
}
