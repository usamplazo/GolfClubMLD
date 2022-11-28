using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Text;

namespace GolfClubMLD.Models.EFRepository
{
    public class AccountRepository : IAccountRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();
        public Task<int> CreateCredCard()
        {
            throw new NotImplementedException();
        }

        public Task<CreditCardBO> GetCredCardById(int credCardId)
        {
            throw new NotImplementedException();
        }
        public async Task<string> HashPassword(string pass)
        {
            MD5CryptoServiceProvider encryptor = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] encryptedValueBytes = encryptor.ComputeHash(encoder.GetBytes(pass));
            StringBuilder encryptedValueBuilder = new StringBuilder();
            for(int i = 0;i<encryptedValueBytes.Length; i++)
            {
                encryptedValueBuilder.Append(encryptedValueBytes[i].ToString("x2"));
                
            }
            string encryptedValue = encryptedValueBuilder.ToString();

            return encryptedValue;
        }
        public async Task<CustomerBO> LoginCustomer(string email, string pass)
        {
            string genMD5pass = await HashPassword(pass);
                Task<CustomerBO> selCust = _gcEntities.Customer
                    .Select(c => c)
                    .Where(c => (c.email == email) && (c.pass == genMD5pass) && (c.isActv == true))
                    .Include(cd => cd.CreditCard)
                    .ProjectTo<CustomerBO>()
                    .FirstOrDefaultAsync();

            return await selCust;
        }

        public async Task RemoveAcc(int custId)
        {
            Customer cust = _gcEntities.Customer.FirstOrDefault(c => c.id == custId);
            if (cust != null)
            {
                cust.isActv = false;
                try
                {
                    _gcEntities.SaveChanges();

                }
                catch (Exception ex)
                {
                    //ISPRAVITI ERROR HANDELING
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task UpdateAcc(int custId)
        {
            Customer cust = _gcEntities.Customer.FirstOrDefault(c => c.id == custId);
            if (cust != null)
            {
                cust.isActv = false;
                try
                {
                    _gcEntities.SaveChanges();
                 
                }
                catch (Exception ex)
                {
                    //ISPRAVITI ERROR HANDELING
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}