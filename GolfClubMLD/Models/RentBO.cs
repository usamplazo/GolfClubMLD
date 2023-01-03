using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class RentBO
    {
        public int Id { get; set; }
        public DateTime BillDate { get; set; }
        public float TotPrice { get; set; }
        public int CourTrmId { get; set; }
        public CourseTermBO CourTrm { get; set; }
        public int CustId { get; set; }
        public UsersBO Customer { get; set; }
    }
}