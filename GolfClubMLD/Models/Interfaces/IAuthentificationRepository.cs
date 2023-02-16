using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface IAuthentificationRepository
    {
        Task<UsersBO> LoginUser(string email, string pass);
        string HashPassword(string pass);
        bool RegisterCustomer(UserAndCreditCardViewModel custCredCard);
        bool CheckExistingCustomer(string email, string username);
        bool CheckExistingCreditCard(long creditCardNum);
        bool UpdateCustomerCredCard(Users cust);
        void RemoveAcc(int custId);
        List<string> GetRoles(string eml);

    }
}
