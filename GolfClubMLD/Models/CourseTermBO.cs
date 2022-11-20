using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CourseTermBO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public GolfCourseBO GolfCourse { get; set; }
        public int TermId { get; set; }
        public TermBO Term { get; set; }
        public string dayOfW { get; set; }
    }
}