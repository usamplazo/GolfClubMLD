﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface IAdminRepository
    {
        List<UsersBO> GetAllUsers();

        bool RegisterManager(UsersBO manager);
    }
}
