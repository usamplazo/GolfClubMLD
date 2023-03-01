using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class EquipmentBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Morate uneti naziv opreme")]
        [Display(Name = "Naziv")]
        public string Name { get; set; }
        public string PicUrl { get; set; }

        [Required(ErrorMessage = "Morate uneti opis")]
        [Display(Name = "Opis")]
        public string Descr { get; set; }

        [Required(ErrorMessage = "Morate uneti cenu")]
        [Display(Name = "Cena")]
        //[RegularExpression(@"^\d$", ErrorMessage = "Neispravan unos cene")]
        public double Price { get; set; }

        [Display(Name = "Oznaka tipa")]
        public int EquipmentTypId { get; set; }
        public EquipmentTypesBO EquipmentTypes { get; set; }
    }
}