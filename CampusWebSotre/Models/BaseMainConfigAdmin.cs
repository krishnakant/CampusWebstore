using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using IWEB;

namespace CampusWebSotre.Models
{
    public class BaseMainConfigAdmin
    {
        BuyBack _a;

        AdminConfig _b;
           
        Configuration c = WebConfigurationManager.OpenWebConfiguration("~");

        public BaseMainConfigAdmin()
        {
            //
            // TODO: Add constructor logic here
            //        
            if (c.Sections["CWSAdminConfig"] != null)
            {
                _b = c.Sections["CWSAdminConfig"] as AdminConfig;
            }
            if (c.Sections["buybackConfig"] != null)
            {
                _a = c.Sections["buybackConfig"] as BuyBack;
            }
        }

        public BuyBack IwebBuyBackConfig
        {
            get { return _a; }
        }
        public AdminConfig IwebConfigAdmin
        {
            get { return _b; }
        }
        public void SaveValues()
        {
            c.Save(ConfigurationSaveMode.Full);
        }
    }
}