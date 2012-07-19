using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusWebSotre.Models
{
    public class PageViewModel
    {
        #region "Properties"

        public string PageName { get; set; }

        public string PageDesc { get; set; }

        public bool ChkPolicy { get; set; }

        public string IsSelected { get; set; }

        public bool IsBackup { get; set; }
        [UIHint("RichEditorControl")]
        public string PageContent { get; set; }

        #endregion

    }
}