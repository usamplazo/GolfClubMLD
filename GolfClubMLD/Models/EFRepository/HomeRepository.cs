using AutoMapper;
using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models.EFRepository
{
    public class HomeRepository : IHomeRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();
        public IEnumerable<GolfCourseBO> GetAllCourses()
        {
            List<GolfCourseBO> allCourses = _gcEntities.GolfCourse
                 .Select(c => c)
                 .Include(t => t.CourseType)
                 .ProjectTo<GolfCourseBO>()
                 .ToList();

            return allCourses;
           
        }
        public GolfCourseBO GetCourseById(int id)
        {
            GolfCourseBO specCourse = _gcEntities.GolfCourse
                .Where(c => c.id == id)
                .Select(c => c)
                .Include(t => t.CourseType)
                .ProjectTo<GolfCourseBO>()
                .FirstOrDefault();

            return specCourse;
        }


        public IEnumerable<GolfCourseBO> GetCoursesBySearch(string search)
        {
            List<GolfCourseBO> coursesBySeacrh = _gcEntities.GolfCourse
                .Where(c => c.name.Contains(search.ToLower()))
                .Select(c => c)
                .Include(t => t.CourseType)
                .ProjectTo<GolfCourseBO>()
                .ToList();

            return coursesBySeacrh;
        }
        public IEnumerable<CourseTermBO> GetAllCourseTerm()
        {
            List<CourseTermBO> courseTerms = _gcEntities.CourseTerm
                .Select(ct => ct)
                .Include(c => c.GolfCourse)
                .Include(t => t.Term)
                .ProjectTo<CourseTermBO>()
                .ToList();

            return courseTerms;


        }

        public IEnumerable<EquipmentBO> GetAllEquipment()
        {
            List<EquipmentBO> allEquip = _gcEntities.Equipment
                 .Select(e => e)
                 .Include(et => et.EquipmentTypes)
                 .ProjectTo<EquipmentBO>()
                 .ToList();

            return allEquip;
        }


        public EquipmentBO GetEquipmentById(int id)
        {
            EquipmentBO equip = _gcEntities.Equipment
                 .Where(e => e.id == id)
                 .Select(e => e)
                 .Include(et => et.EquipmentTypes)
                 .ProjectTo<EquipmentBO>()
                 .FirstOrDefault();

            return equip;

        }

    }
}