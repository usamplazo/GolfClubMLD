using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class RentDetailBO
    {
        public int Id { get; set; }
        public int EquipId { get; set; }
        public EquipmentBO Eqipment { get; set; }
        public int RentId { get; set; }
        public RentBO Rent { get; set; }

    }
}