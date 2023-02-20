using GolfClubMLD.Models.ViewModel;
using System;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface ICustomerRepository
    {
        DateTime CalculateDate(DateTime now);
        bool EditCustomerData(UserAndCreditCardViewModel ccvm);
        bool DeactCustomer(int custId);
        void SendEmail(string emailTo, string name);
    }
}
