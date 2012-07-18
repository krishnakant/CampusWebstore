using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Models
{
    public class FacultyTermViewModel
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public string StoreId { get; set; }
        public string StoreDesc { get; set; }
    }
}