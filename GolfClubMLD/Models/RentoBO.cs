using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class RentoBO
    {
        public int Id { get; set; }
        public DateTime ConfirmDate { get; set; }
        public string DayOfWeek { get; set; }
        public float TotalPrice { get; set; }
        public int RentItemId { get; set; }

        public List<RentItemBO> RentItems { get; set; }

    }
}