using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CreditCardBO
    {
        public int Id { get; set; }
        public long CarNum { get; set; }
        public string Own { get; set; }
        public int Expiry { get; set; }
        public int CVV { get; set; }
    }
}