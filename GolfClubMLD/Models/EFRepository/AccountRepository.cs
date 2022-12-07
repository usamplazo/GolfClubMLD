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
using AutoMapper;
using GolfClubMLD.Models.ViewModel;

namespace GolfClubMLD.Models.EFRepository
{
    public class AccountRepository : IAccountRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();
      
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

        public bool CheckExistingCustomer(string email)
        {
            Task<CustomerBO> findCust =  _gcEntities.Customer
                       .Select(c => c)
                       .Where(c => c.email == email)
                       .ProjectTo<CustomerBO>()
                       .FirstOrDefaultAsync();

            if (findCust.Result == null)
                return false;
            return true;
        }
        public async Task<bool> RegisterCustomer(CustomerCreditCardViewModel custCredCard)
        {
            if (CheckExistingCustomer(custCredCard.Cust.Email))
            {
                return false;
            }
            try
            {
                var hashPass = await HashPassword(custCredCard.Cust.Pass);

                Customer cust = new Customer { 
                    email = custCredCard.Cust.Email,
                    pass = hashPass,
                    fname = custCredCard.Cust.Fname,
                    lname = custCredCard.Cust.Lname,
                    phone = custCredCard.Cust.Phone,
                    profPic = "i",
                    isActv = true

            };

                CreditCard cc = new CreditCard
                {
                    carNum = custCredCard.CredCard.CarNum,
                    own = custCredCard.CredCard.Own,
                    expiry = custCredCard.CredCard.Expiry,
                    cvv = custCredCard.CredCard.CVV
                };
                _gcEntities.CreditCard.Add(cc);
                _gcEntities.Customer.Add(cust);
                _gcEntities.SaveChanges();

            }
            catch(Exception ex)
            {
                //exception handeling
                return false;
            }

            if (UpdateCustomerCredCard())
                return true;

            return false;


        }
        public bool AddCustomerCreditCard(CreditCardBO ccBo)
        {
            
                try
                {
                CreditCard cc = new CreditCard
                {
                    carNum = ccBo.CarNum,
                    own = ccBo.Own,
                    expiry = ccBo.Expiry,
                    cvv = ccBo.CVV
                };
                    _gcEntities.CreditCard.Add(cc);
                    _gcEntities.SaveChanges();
                }
                catch(Exception ex)
                {
                    //error handeling
                     return false;
                }

            if(UpdateCustomerCredCard())
                return true;

            return false;

        }
        public bool UpdateCustomerCredCard()
        {
            try
            {

                Customer cust = _gcEntities.Customer.Select(c => c).OrderByDescending(c => c.id).FirstOrDefault();
                CreditCard cc = _gcEntities.CreditCard.Select(cr => cr).OrderByDescending(cr => cr.id).FirstOrDefault(); ;
                if (cust is null || cc is null)
                    return false;
                cust.credCardId = cc.id;
                _gcEntities.SaveChanges();
            }
            catch(Exception ex)
            {
                //error handeling
                return false;
            }
            return true;
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