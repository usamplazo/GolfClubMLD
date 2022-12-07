using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface IAccountRepository
    {
        #region Customer
        Task<string> HashPassword(string pass);
        Task<CustomerBO> LoginCustomer(string email, string pass);
        bool CheckExistingCustomer(string email);
        Task<bool> RegisterCustomer(CustomerCreditCardViewModel custCredCard);
        Task RemoveAcc(int custId);
        Task UpdateAcc(int custId);

        bool AddCustomerCreditCard(CreditCardBO ccBo);
        Task<CreditCardBO> GetCredCardById(int credCardId);
        bool UpdateCustomerCredCard();

        #endregion
    }
}
