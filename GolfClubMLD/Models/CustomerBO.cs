using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CustomerBO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool isActive { get; set; }
        public int CreditCardId { get; set; }
        public CreditCardBO CreditCard { get; set; }
    }
}