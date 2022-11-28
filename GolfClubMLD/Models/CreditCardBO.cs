using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CreditCardBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Morate uneti broj kartice")]
        [Display(Name = "Broj kartice")]
        [RegularExpression(@"^4[0-9]{15}", ErrorMessage = "Neispravan broj kartice")]
        [DataType(DataType.CreditCard)]
        public long CarNum { get; set; }

        [Required(ErrorMessage = "Morate uneti vlasnika kartice")]
        [Display(Name = "Vlasnik kartice")]
        public string Own { get; set; }

        [Required(ErrorMessage = "Morate uneti datum isteka kartice")]
        [Display(Name = "Datum isteka")]
        [RegularExpression(@"^[0-9]{2}\/[0-9]{2}", ErrorMessage = "Nesipravan datum")]
        public string Expiry { get; set; }

        [Required(ErrorMessage = "Morate uneti CVV")]
        [Display(Name = "CVV")]
        [RegularExpression(@"^[0-9]{3}")]
        public int CVV { get; set; }
    }
}