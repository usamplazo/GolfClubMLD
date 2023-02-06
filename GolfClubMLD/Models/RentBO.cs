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
        public double TotPrice { get; set; }
        public int CourTrmId { get; set; }
        public CourseTermBO CourseTerm { get; set; }
        public int CustId { get; set; }
        public UsersBO Users { get; set; }
        public DateTime RentDate { get; set; }
        public IList<RentItemsBO> RentItems { get; set; }
    }
}