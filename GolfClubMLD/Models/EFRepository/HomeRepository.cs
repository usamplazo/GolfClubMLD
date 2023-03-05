using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.EFRepository
{
    public class HomeRepository : BaseRepository, IHomeRepository
    {
        private GolfClubMldDBEntities _gcEntities = new GolfClubMldDBEntities();

        #region Courses
       
        public async Task<List<GolfCourseBO>> GetCoursesBySearch(string search)
        {
            Task<List<GolfCourseBO>> coursesBySeacrh = _gcEntities.GolfCourse
                                                                    .Where(c => c.name.Contains(search.ToLower()))
                                                                    .Include(t => t.CourseType)
                                                                    .ProjectTo<GolfCourseBO>()
                                                                    .ToListAsync();

            return await coursesBySeacrh;
        }

        public async Task<List<GolfCourseBO>> GetCoursesByType(int typeId)
        {
            Task<List<GolfCourseBO>> coursesByType = _gcEntities.GolfCourse
                                                                    .Where(c => c.CourseType.id == typeId)
                                                                    .Include(t => t.CourseType)
                                                                    .ProjectTo<GolfCourseBO>()
                                                                    .ToListAsync();

            return await coursesByType;
        }

        #endregion

        #region Equipment
        public async Task<EquipmentBO> GetEquipmentById(int id)
        {
           Task<EquipmentBO> equip = _gcEntities.Equipment
                                                     .Where(e => e.id == id)
                                                     .Include(et => et.EquipmentTypes)
                                                     .ProjectTo<EquipmentBO>()
                                                     .FirstOrDefaultAsync();

            return await equip;
        }

        public async Task<List<EquipmentBO>> GetEquipmentByType(int typeId)
        {
            Task<List<EquipmentBO>> equipByType = _gcEntities.Equipment
                                                                .Where(e=>e.EquipmentTypes.id == typeId)
                                                                .Include(et=>et.EquipmentTypes)
                                                                .ProjectTo<EquipmentBO>()
                                                                .ToListAsync();

            return await equipByType;
        }

        public async Task<List<EquipmentBO>> GetEquipmentBySearch(string search)
        {
            Task<List<EquipmentBO>> equipBySeacrh = _gcEntities.Equipment
                                                                  .Where(c => c.name.Contains(search.ToLower()))
                                                                  .Include(t => t.EquipmentTypes)
                                                                  .ProjectTo<EquipmentBO>()
                                                                  .ToListAsync();

            return await equipBySeacrh;
        }
        #endregion
    }
}