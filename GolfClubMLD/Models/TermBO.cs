using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class TermBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Morate uneti pocetak termina")]
        [Display(Name = "Pocetka termina")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "Morate uneti kraj termina")]
        [Display(Name = "Kraj termina")]
        public string EndTime { get; set; }
    }
}