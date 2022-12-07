using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models.ViewModel
{
    public class CustomerCreditCardViewModel
    {
        public CustomerBO Cust { get; set; }
        public CreditCardBO CredCard { get; set; }
    }
}