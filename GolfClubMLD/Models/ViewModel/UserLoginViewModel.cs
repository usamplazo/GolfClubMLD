using System.ComponentModel.DataAnnotations;

namespace GolfClubMLD.Models.ViewModel
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Morate uneti email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Neispravan format mail-a")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Morate uneti lozinku")]
        [Display(Name = "Lozinka")]
        [DataType(DataType.Password)]
        //[StringLength(100, ErrorMessage = "Lozinka \"{0}\" mora imati {2} karaktera"), MinLength(5, ErrorMessage = "Lozinka mora imati minimum 5 karaktera")]
        //[RegularExpression(@"^([a-zA-Z0-9@*#]{5,15})$" , ErrorMessage = "Lozinka mora da sadrzi 8 karaktera, velika slova, mala slova i sprecijalne karaktere")]
        public string Pass { get; set; }

    }
}