using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models.ViewModel
{
    public class RentInfoConfirmViewModel
    {
        public UserAndCreditCardViewModel UserAndCredCard { get; set; }
        public GolfCourseBO Course { get; set; }
        public CourseTermBO CorTerm { get; set; }
        public List<EquipmentBO> Equipment { get; set; }
    }
}