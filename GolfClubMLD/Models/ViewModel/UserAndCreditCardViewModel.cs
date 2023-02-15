using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models.ViewModel
{
    public class UserAndCreditCardViewModel
    {
        public UsersBO Cust { get; set; }
        public CreditCardBO CredCard { get; set; }
    }
}