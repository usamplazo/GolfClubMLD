using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CustomerBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Morate uneti email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Neispravan format mail-a")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Morate uneti lozinku")]
        [Display(Name = "Lozinka")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Lozinka \"{0}\" mora imati {2} karaktera"), MinLength(5, ErrorMessage = "Lozinka mora imati minimum 5 karaktera")]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{5,15})$" , ErrorMessage = "Lozinka mora da sadrzi 8 karaktera, velika slova, mala slova i sprecijalne karaktere")]
        public string Pass { get; set; }

        [NotMapped]
        [Required]
        public int ConfPass { get; set; }

        [Required(ErrorMessage = "Morate uneti ime")]
        [Display(Name = "Ime")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "Morate uneti prezime")]
        [Display(Name = "Prezime")]
        public string Lname { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Broj telefona")]
        [Required(ErrorMessage = "Morate uneti broj telefona")]
        [RegularExpression(@"([+]?381)?(6\d{1})\d{7,9}")]
        public string Phone { get; set; }
        public string ProfPic { get; set; }
        public bool IsActv { get; set; }
        public int CredCardId { get; set; }
        public CreditCardBO CreditCard { get; set; }
    }
}