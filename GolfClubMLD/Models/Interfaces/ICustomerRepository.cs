using GolfClubMLD.Models.ViewModel;
using System;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface ICustomerRepository
    {
        DateTime CalculateDate(DateTime now);
        bool EditCustomerData(UserAndCreditCardViewModel ccvm);
        void SendEmail(string emailTo, string name);
    }
}
