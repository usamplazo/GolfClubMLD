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
        public float Price { get; set; }
        public int Amount { get; set; }
        public int EquipmentTypeId { get; set; }
        public EquipmentTypesBO EquipmentType { get; set; }
    }
}