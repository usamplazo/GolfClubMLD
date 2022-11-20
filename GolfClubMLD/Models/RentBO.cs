using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class RentBO
    {
        public int Id { get; set; }
        public DateTime ConDate { get; set; }
        public float TotPrice { get; set; }
        public List<RentDetailBO> RentItems { get; set; }

        public int CourTrmId { get; set; }
        public CourseTermBO CourseTerm { get; set; }
        public string CustEmail { get; set; }
        public CustomerBO Customer { get; set; }


    }
}