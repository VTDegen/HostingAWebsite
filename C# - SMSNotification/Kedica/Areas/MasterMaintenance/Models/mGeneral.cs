using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSNofication.Areas.MasterMaintenance.Models
{
    public class mGeneral
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        public string TypeDesc { get; set; }
        public string Value { get; set; }
    }
}