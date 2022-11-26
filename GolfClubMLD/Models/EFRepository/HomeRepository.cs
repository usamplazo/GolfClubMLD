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
        public Task<List<CourseTypeBO>> GetAllCourseTypes()
        {
            Task<List<CourseTypeBO>> allTypes = _gcEntities.CourseType
                .Select(ct => ct)
                .ProjectTo<CourseTypeBO>()
                .ToListAsync();

            return allTypes;
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
        public async Task<List<GolfCourseBO>> GetCoursesByType(int typeId)
        {
            Task<List<GolfCourseBO>> coursesByType = _gcEntities.GolfCourse
                .Where(c => c.CourseType.id == typeId)
                .Select(c => c)
                .Include(t => t.CourseType)
                .ProjectTo<GolfCourseBO>()
                .ToListAsync();

            return await coursesByType;
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

        public async Task<List<EquipmentTypesBO>> GetAllEquipmentTypes()
        {
            Task<List<EquipmentTypesBO>> allTypes = _gcEntities.EquipmentTypes
          .Select(et=>et)
          .ProjectTo<EquipmentTypesBO>()
          .ToListAsync();

            return await allTypes;
        }


        public async Task<List<EquipmentBO>> GetEquipmentByType(int typeId)
        {
            Task<List<EquipmentBO>> equipByType = _gcEntities.Equipment
                .Where(e=>e.EquipmentTypes.id == typeId)
                .Select(c => c)
                .Include(et=>et.EquipmentTypes)
                .ProjectTo<EquipmentBO>()
                .ToListAsync();

            return await equipByType;
        }

        public async Task<List<EquipmentBO>> GetEquipmentBySearch(string search)
        {
            Task<List<EquipmentBO>> equipBySeacrh = _gcEntities.Equipment
              .Where(c => c.name.Contains(search.ToLower()))
              .Select(c => c)
              .Include(t => t.EquipmentTypes)
              .ProjectTo<EquipmentBO>()
              .ToListAsync();

            return await equipBySeacrh;
        }
    }
}