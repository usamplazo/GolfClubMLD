using System.Collections.Generic;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    public interface IHomeRepository
    {
        #region Courses
        Task<List<GolfCourseBO>> GetAllCourses();
        Task<GolfCourseBO> GetCourseById(int id);
        Task<List<CourseTypeBO>> GetAllCourseTypes();
        Task<List<GolfCourseBO>> GetCoursesBySearch(string search);
        Task<List<GolfCourseBO>> GetCoursesByType(int typeId);
        Task<List<CourseTermBO>> GetAllCourseTerm();
        #endregion

        #region Equipment
        // CourseTermBO GetCourseTermById(int id);
        Task<List<EquipmentTypesBO>> GetAllEquipmentTypes();
        Task<EquipmentBO> GetEquipmentById(int id);
        Task<List<EquipmentBO>> GetEquipmentByType(int typeId);
        Task<List<EquipmentBO>> GetEquipmentBySearch(string search);
        #endregion
    }
}
