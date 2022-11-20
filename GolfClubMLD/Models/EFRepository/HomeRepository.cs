using AutoMapper;
using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GolfClubMLD.Models.EFRepository
{
    public class HomeRepository : IHomeRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();
        public async Task<List<GolfCourseBO>> GetAllCourses()
        {
            Task<List<GolfCourseBO>> allCourses = _gcEntities.GolfCourse
                 .Select(c => c)
                 .Include(t => t.CourseType)
                 .ProjectTo<GolfCourseBO>()
                 .ToListAsync();

            return await allCourses;
           
        }
        public async Task<GolfCourseBO> GetCourseById(int id)
        {
            Task<GolfCourseBO> specCourse = _gcEntities.GolfCourse
                .Where(c => c.id == id)
                .Select(c => c)
                .Include(t => t.CourseType)
                .ProjectTo<GolfCourseBO>()
                .FirstOrDefaultAsync();

            return await specCourse;
        }


        public async Task<List<GolfCourseBO>> GetCoursesBySearch(string search)
        {
            Task<List<GolfCourseBO>> coursesBySeacrh = _gcEntities.GolfCourse
                .Where(c => c.name.Contains(search.ToLower()))
                .Select(c => c)
                .Include(t => t.CourseType)
                .ProjectTo<GolfCourseBO>()
                .ToListAsync();

            return await coursesBySeacrh;
        }
        public async Task<List<CourseTermBO>> GetAllCourseTerm()
        {
            Task<List<CourseTermBO>> courseTerms = _gcEntities.CourseTerm
                .Select(ct => ct)
                .Include(c => c.GolfCourse)
                .Include(t => t.Term)
                .ProjectTo<CourseTermBO>()
                .ToListAsync();

            return await courseTerms;
        }

        public async Task<List<EquipmentBO>> GetAllEquipment()
        {
            Task<List<EquipmentBO>> allEquip = _gcEntities.Equipment
                 .Select(e => e)
                 .Include(et => et.EquipmentTypes)
                 .ProjectTo<EquipmentBO>()
                 .ToListAsync();

            return await allEquip;
        }


        public async Task<EquipmentBO> GetEquipmentById(int id)
        {
           Task<EquipmentBO> equip = _gcEntities.Equipment
                 .Where(e => e.id == id)
                 .Select(e => e)
                 .Include(et => et.EquipmentTypes)
                 .ProjectTo<EquipmentBO>()
                 .FirstOrDefaultAsync();

            return await equip;
        }

    }
}