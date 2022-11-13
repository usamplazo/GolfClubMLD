using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class AdminBO:UserBO
    {
        public string AdminUsername { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
    }
}