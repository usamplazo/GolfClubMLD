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
            
            CreateMap<GolfCourse, GolfCourseBO>().ReverseMap().ForMember(gc=>gc.CourseTerms, ri => ri.Ignore());
            CreateMap<Equipment, EquipmentBO>().ReverseMap().ForMember(e => e.RentItems, ri => ri.Ignore());
            CreateMap<Term, TermBO>().ReverseMap().ForMember(t=>t.CourseTerms, ri => ri.Ignore());
            CreateMap<CourseTerm, CourseTermBO>();
            CreateMap<CourseType, CourseTypeBO>();
            CreateMap<CreditCard, CreditCardBO>();
            CreateMap<Rent, RentBO>();
            CreateMap<RentItem, RentItemsBO>();
            CreateMap<User, UsersBO>().ReverseMap().ForMember(c=>c.id, opt=>opt.Ignore());
        }
    }
}