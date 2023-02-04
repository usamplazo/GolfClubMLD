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
        string HashPassword(string pass);
        Task<UsersBO> LoginUser(string email, string pass);
        bool CheckExistingCustomer(string email, string username);
        Task<bool> RegisterCustomer(CustomerCreditCardViewModel custCredCard);
        void RemoveAcc(int custId);
        void UpdateAcc(int custId);
        bool CheckExistingCreditCard(long creditCardNum);
        bool UpdateCustomerCredCard(Users cust);
        List<string> GetRoles(string eml);

    }
}
