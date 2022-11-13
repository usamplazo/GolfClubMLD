using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models
{
    public class CourseTypeBO
    {
        public int Id { get; set; }
        public bool Obstacles { get; set; }
        public bool NightPlay { get; set; }
        public bool Cleaning { get; set; }
        public bool SprinklerSystem { get; set; }
        public string Description { get; set; }
        public float Par { get; set; }
        public int NumberOfHoles { get; set; }
        public int Surface { get; set; }

    }
}