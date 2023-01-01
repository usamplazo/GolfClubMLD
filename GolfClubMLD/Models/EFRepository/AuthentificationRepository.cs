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
    public class AuthentificationRepository : IAuthentificationRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();
      
        public async Task<CreditCardBO> GetCredCardById(int credCardId)
        {
            CreditCard credCard = await _gcEntities.CreditCard.FirstOrDefaultAsync(cc => cc.id == credCardId);
            CreditCardBO crCd = Mapper.Map<CreditCardBO>(credCard);
            return crCd;
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
        public async Task<UsersBO> LoginCustomer(string credential, string pass)
        {
            string genMD5pass = await HashPassword(pass);
                Task<UsersBO> selCust = _gcEntities.Users
                    .Select(c => c)
                    .Where(c => (c.email == credential || c.username == credential) && (c.pass == genMD5pass) && (c.isActv == true))
                    .Include(cd => cd.CreditCard)
                    .ProjectTo<UsersBO>()
                    .FirstOrDefaultAsync();

            return await selCust;
        }

        public bool CheckExistingCustomer(string email, string username)
        {
            Task<UsersBO> findCust =  _gcEntities.Users
                       .Select(c => c)
                       .Where(c => c.email == email || c.username == username)
                       .ProjectTo<UsersBO>()
                       .FirstOrDefaultAsync();

            if (findCust.Result == null)
                return false;
            return true;
        }
        public async Task<bool> RegisterCustomer(CustomerCreditCardViewModel custCredCard)
        {
            if (CheckExistingCustomer(custCredCard.Cust.Email, custCredCard.Cust.Username) || CheckExistingCreditCard(custCredCard.CredCard.CarNum))
            {
                return false;
            }
            try
            {
                var hashPass = await HashPassword(custCredCard.Cust.Pass);

                Users cust = new Users
                {
                    email = custCredCard.Cust.Email,
                    pass = hashPass,
                    fname = custCredCard.Cust.Fname,
                    lname = custCredCard.Cust.Lname,
                    phone = custCredCard.Cust.Phone,
                    profPic = "noPicture", //default picture location
                    isActv = true,
                    roleId = 1
                };

                CreditCard cc = new CreditCard
                {
                    carNum = custCredCard.CredCard.CarNum,
                    own = custCredCard.CredCard.Own,
                    expiry = custCredCard.CredCard.Expiry,
                    cvv = custCredCard.CredCard.CVV
                };
                _gcEntities.CreditCard.Add(cc);
                _gcEntities.SaveChanges();

                if (UpdateCustomerCredCard(cust))
                    return true;

            }
            catch (Exception ex)
            {
                //exception handeling
                return false;
            }

            return false;
        }

        public bool CheckExistingCreditCard(long creditCardNum)
        {
            Task<CreditCardBO> findCredCard = _gcEntities.CreditCard
                    .Select(cc => cc)
                    .Where(cc => cc.carNum == creditCardNum)
                    .ProjectTo<CreditCardBO>()
                    .FirstOrDefaultAsync();
            if (findCredCard.Result == null)
                return false;
            return true;
            
        }

        public bool UpdateCustomerCredCard(Users cust)
        {
            try
            {
                CreditCard cc = _gcEntities.CreditCard.Select(cr => cr).OrderByDescending(cr => cr.id).FirstOrDefault(); ;
                if (cust is null || cc is null)
                    return false;
                cust.credCardId = cc.id;
                _gcEntities.Users.Add(cust);
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
            Users cust = _gcEntities.Users.FirstOrDefault(c => c.id == custId);
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
            Users cust = _gcEntities.Users.FirstOrDefault(c => c.id == custId);
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
        public List<string> GetRoles(string username)
        {
            Users user = _gcEntities.Users.Select(u=>u)
                                          .FirstOrDefault(u=>u.username == username);
            List<string> result = new List<string>();
            result.Add(user.Role.name);
            return result;
        }
    }
}