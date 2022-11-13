using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CustomerBO:UserBO
    {
        public string PhoneNumber { get; set; }
        public bool isActive { get; set; }
        public int RoleId { get; set; }
        public int CreditCardId { get; set; }
        public CreditCardBO CreditCard { get; set; }
    }
}