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
        public string PicUrl { get; set; }
        public double Price { get; set; }
        public int CorTypId { get; set; }
        public CourseTypeBO CourseType { get; set; }

    }
}