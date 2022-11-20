using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CourseTypeBO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Obstcl { get; set; }
        public bool NightPly { get; set; }
        public bool Cleaning { get; set; }
        public bool SprnklSys { get; set; }
        public string Descr { get; set; }
        public double Par { get; set; }
        public int NumOfHoles { get; set; }
        public int Surfc { get; set; }

    }
}