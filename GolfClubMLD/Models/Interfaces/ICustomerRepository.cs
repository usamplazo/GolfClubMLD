using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface ICustomerRepository
    {
        DateTime CalculateDate(DateTime now);
        void SendEmail(string emailTo, string name);
        bool EditCustomerData(UserAndCreditCardViewModel ccvm);
        string HashPassword(string pass);
        bool DeactCustomer(int custId);
    }
}
