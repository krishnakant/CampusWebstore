using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
   public class CourseSectionModel
    {
        #region "Properties"

        public string Comment { get; set; }

        public string Class { get; set; }

        public IEnumerable<ItemModel> ItemModels { get; set; }

        #endregion 

    }
}
