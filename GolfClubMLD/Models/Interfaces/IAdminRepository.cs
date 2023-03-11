using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    public interface IAdminRepository
    {
        List<UsersBO> GetAllUsers();

        bool RegisterManager(UsersBO manager);

        bool EditManagerData(UsersBO manager);

        bool EditCourse(GolfCourseBO course);

        bool DelCourse(int courseId);

        bool CreateEquipment(EquipmentBO equip);

        bool CreateCourse(GolfCourseBO course);

        bool CreateTerm(TermBO term);

        int GetTermId();

        bool CreateCourseTerm(int termId, int courseId, string day);
    }
}
