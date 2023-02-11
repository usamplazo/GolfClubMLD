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
            CreateMap<Equipment, EquipmentBO>().ReverseMap().ForMember(e => e.RentItems, ri => ri.Ignore()); ;
            CreateMap<CourseTerm, CourseTermBO>();
            CreateMap<CourseType, CourseTypeBO>();
            CreateMap<CreditCard, CreditCardBO>();
            CreateMap<Rent, RentBO>();
            CreateMap<RentItems, RentItemsBO>();
            CreateMap<Users, UsersBO>().ReverseMap().ForMember(c=>c.id, opt=>opt.Ignore());
        }
    }
}