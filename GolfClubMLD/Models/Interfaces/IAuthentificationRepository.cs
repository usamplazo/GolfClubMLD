using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface IAuthentificationRepository
    {
        #region Customer
        Task<string> HashPassword(string pass);
        Task<UsersBO> LoginCustomer(string email, string pass);
        bool CheckExistingCustomer(string email, string username);
        Task<bool> RegisterCustomer(CustomerCreditCardViewModel custCredCard);
        Task RemoveAcc(int custId);
        Task UpdateAcc(int custId);

        bool CheckExistingCreditCard(long creditCardNum);
        Task<CreditCardBO> GetCredCardById(int credCardId);
        bool UpdateCustomerCredCard(Users cust);
        List<string> GetRoles(string eml);
            #endregion
        }
}
