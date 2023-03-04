using System.ComponentModel.DataAnnotations;

namespace GolfClubMLD.Models
{
    public class GolfCourseBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Morate uneti naziv terena")]
        [Display(Name = "Naziv")]
        public string Name { get; set; }
        public string PicUrl { get; set; }

        [Required(ErrorMessage = "Morate uneti cenu terena")]
        [Display(Name = "Cena")]
        public double Price { get; set; }
        public int CorTypId { get; set; }
        public CourseTypeBO CourseType { get; set; }

    }
}