using AutoMapper.QueryableExtensions;
using GolfClubMLD.Controllers;
using GolfClubMLD.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace GolfClubMLD.Models.EFRepository
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        private GolfClubMldDBEntities _adminEntities = new GolfClubMldDBEntities();
        private readonly ILogger<CustomerController> _logger;
        public List<UsersBO> GetAllUsers()
        {
            List<UsersBO> allUsers = _adminEntities.Users
                                                        .Where(u => u.isActv && (u.roleId == 1 || u.roleId == 2))
                                                        .ProjectTo<UsersBO>()
                                                        .ToList();
            // 1 - Customer
            // 2 - Manager

            return allUsers;
        }
        public bool RegisterManager(UsersBO manager)
        {
            if (manager is null)
                return false;

            try
            {
                Users managerdb = new Users()
                {
                    email = manager.Email,
                    username = manager.Username,
                    pass = manager.Pass,
                    fname = manager.Fname,
                    lname = manager.Lname,
                    phone = manager.Phone,
                    profPic = manager.ProfPic,
                    isActv = true,
                    roleId = manager.RoleId
                };
                _adminEntities.Users.Add(managerdb);
                _adminEntities.SaveChanges();
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
            return true;
        }
    }
}