using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CreditCardBO
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public long CardNumber { get; set; }
        public int ExpiryDate { get; set; }
        public int CVV { get; set; }
    }
}