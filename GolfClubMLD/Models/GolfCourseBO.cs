using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class GolfCourseBO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int CourseTypeId { get; set; }
        public CourseTypeBO CourseType { get; set; }

    }
}