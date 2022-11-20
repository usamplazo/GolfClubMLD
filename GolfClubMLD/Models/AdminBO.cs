using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class AdminBO
    {
        [Key]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Phone { get; set; }
        public string ProfPic { get; set; }
    }
}