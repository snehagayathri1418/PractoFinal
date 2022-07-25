using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PractoFinal.Models
{
    public class Display
    {
        [Key]
        public int Doc_id { get; set; }
        public int Loc_id { get; set; }
        public int Spec_id { get; set; }
        public string Doc_name { get; set; }
        public int Doc_exp { get; set; }
        public decimal Doc_fees { get; set; }
        public bool Status { get; set; }


        public string Loc_name { get; set; }

        public string Spec_name { get; set; }

        public int TimeSlotID { get; set; }
        public string Time { get; set; }
    }
}