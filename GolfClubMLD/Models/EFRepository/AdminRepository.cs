using AutoMapper.QueryableExtensions;
using GolfClubMLD.Controllers;
using GolfClubMLD.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}