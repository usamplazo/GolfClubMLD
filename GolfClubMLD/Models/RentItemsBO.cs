using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class RentItemsBO
    {
        public int Id { get; set; }
        public int EquipId { get; set; }
        public EquipmentBO Eqipment { get; set; }
        public double Price { get; set; }
        public int RentId { get; set; }

    }
}