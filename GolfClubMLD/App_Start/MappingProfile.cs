using AutoMapper;
using GolfClubMLD.Models;
using GolfClubMLD.Models.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            
            CreateMap<GolfCourse, GolfCourseBO>();
            CreateMap<Equipment, EquipmentBO>();
            CreateMap<CourseTerm, CourseTermBO>();
        }
    }
}