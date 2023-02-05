using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class EquipmentBO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }
        public string Descr { get; set; }
        public double Price { get; set; }
        public int EquipmentTypId { get; set; }
        public EquipmentTypesBO EquipmentTypes { get; set; }
    }
}