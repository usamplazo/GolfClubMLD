using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    public interface IBaseRepository
    {

        UsersBO GetCustomerById(int custId);
        Task<List<CourseTermBO>> CheckForRentCourses(List<CourseTermBO> courseTerms, int courseId);
        CourseTermBO SelectCourseTermById(int ctId);
        Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId);
        List<EquipmentBO> GetSelEquipmentById(int[] selEqip);
        CustomerCreditCardViewModel GetCustomerCCById(int id);
        GolfCourseBO SelectCourseById(int id);
        CourseTermBO SelectTermById(int id);
        bool SaveRent(int ctId, int custId, DateTime rentDte);
        bool SaveRentItems(int ctId, int custId, int[] equipIds);

    }
}
