using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    public interface IHomeRepository
    {
        IEnumerable<GolfCourseBO> GetAllCourses();
        GolfCourseBO GetCourseById(int id);
        IEnumerable<GolfCourseBO> GetCoursesBySearch(string search);
        IEnumerable<CourseTermBO> GetAllCourseTerm();

        // CourseTermBO GetCourseTermById(int id);
        IEnumerable<EquipmentBO> GetAllEquipment();

        EquipmentBO GetEquipmentById(int id);
    }
}
