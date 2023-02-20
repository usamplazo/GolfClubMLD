using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using GolfClubMLD.Models.ViewModel;
using System.Data.Entity.Validation;
using GolfClubMLD.Controllers;
using Microsoft.Extensions.Logging;

namespace GolfClubMLD.Models.EFRepository
{
    public class AuthentificationRepository : BaseRepository, IAuthentificationRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();
        private readonly ILogger<CustomerController> _logger;
        private readonly string _profileImagePath = "~/Images/ProfileImages/";

        public AuthentificationRepository() { }
        public AuthentificationRepository(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        public async Task<UsersBO> LoginUser(string credential, string pass)
        {
            string genMD5pass = HashPassword(pass.Trim());
                Task<UsersBO> selCust = _gcEntities.Users
                    .Where(c => (c.email == credential) && (c.pass == genMD5pass) && (c.isActv == true))
                    .Include(cd => cd.CreditCard)
                    .ProjectTo<UsersBO>()
                    .FirstOrDefaultAsync();

            return await selCust;
        }

        public bool RegisterCustomer(UserAndCreditCardViewModel custCredCard)
        {
            if (CheckExistingCustomer(custCredCard.Cust.Email, custCredCard.Cust.Username) 
                                    || CheckExistingCreditCard(custCredCard.CredCard.CarNum))
            {
                return false;
            }
            try
            {
                string hashPass = HashPassword(custCredCard.Cust.Pass);
                if (string.IsNullOrWhiteSpace(hashPass))
                    return false;

                Users cust = new Users
                {
                    email = custCredCard.Cust.Email,
                    username = custCredCard.Cust.Username,
                    pass = hashPass,
                    fname = custCredCard.Cust.Fname,
                    lname = custCredCard.Cust.Lname,
                    phone = custCredCard.Cust.Phone,
                    profPic = "", //default picture location
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
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    _logger.LogError("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        _logger.LogError("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: Authentification => RegisterCustomer " + ex);
            }

            return false;
        }

        public bool CheckExistingCustomer(string email, string username)
        {
            Task<UsersBO> findCust = _gcEntities.Users
                                                .Where(c => c.email == email || c.username == username)
                                                .ProjectTo<UsersBO>()
                                                .FirstOrDefaultAsync();

            if (findCust.Result == null)
                return false;

            return true;
        }

        public bool CheckExistingCreditCard(long creditCardNum)
        {
            Task<CreditCardBO> findCredCard = _gcEntities.CreditCard
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
                if (cust is null)
                    return false;

                CreditCard cc = _gcEntities.CreditCard
                                            .OrderByDescending(cr => cr.id)
                                            .FirstOrDefault();

                if (cc is null)
                    return false;

                cust.credCardId = cc.id;
                cust.roleId = 1;
                cust.credCardId = cc.id;

                _gcEntities.Users.Add(cust);
                _gcEntities.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    _logger.LogError("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        _logger.LogError("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: Authentification => UpdateCustomerCredCard " + ex);
            }
            return true;
        }

        public void RemoveAcc(int custId)
        {
            Users cust = _gcEntities.Users.FirstOrDefault(c => c.id == custId);
            if (cust is null)
            {
                return;
            }

            cust.isActv = false;
            try
            {
                _gcEntities.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    _logger.LogError("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        _logger.LogError("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: Authentification => RemoveAcc " + ex);
            }
        }

        public List<string> GetRoles(string username)
        {
            Users user = _gcEntities.Users
                                        .FirstOrDefault(u=>u.username == username);

            List<string> result = new List<string>
            {
                user.Role.name
            };

            return result;
        }
    }
}