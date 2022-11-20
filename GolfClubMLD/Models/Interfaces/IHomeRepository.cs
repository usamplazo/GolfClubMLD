using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    public interface IHomeRepository
    {
        Task<List<GolfCourseBO>> GetAllCourses();
        Task<GolfCourseBO> GetCourseById(int id);
        Task<List<GolfCourseBO>> GetCoursesBySearch(string search);
        Task<List<CourseTermBO>> GetAllCourseTerm();

        // CourseTermBO GetCourseTermById(int id);
        Task<List<EquipmentBO>> GetAllEquipment();

        Task<EquipmentBO> GetEquipmentById(int id);
    }
}
