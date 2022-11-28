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
        Task RemoveAcc(int custId);
        Task UpdateAcc(int custId);

        Task<int> CreateCredCard();
        Task<CreditCardBO> GetCredCardById(int credCardId);

        #endregion
    }
}
