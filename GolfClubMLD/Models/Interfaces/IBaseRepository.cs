using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace GolfClubMLD.Models.Interfaces
{
    public interface IBaseRepository
    {
        #region Customer

        UsersBO GetUserById(int custId);
        UsersBO GetUserByEmail(string email);
        Task<CreditCardBO> GetCredCardById(int credCardId);
        CreditCardBO GetCustomerCCById(int id);
        string HashPassword(string pass);
        string GetImportedProfilePicture(HttpPostedFileBase file, string defPath);
        bool DeactCustomer(int custId);

        #endregion

        #region CourseTerm
        Task<List<GolfCourseBO>> GetAllCourses();
        Task<List<CourseTypeBO>> GetAllCourseTypes();
        Task<GolfCourseBO> GetCourseById(int id);
        Task<List<CourseTermBO>> CheckForRentCourses(List<CourseTermBO> courseTerms, int courseId);
        CourseTermBO SelectCourseTermById(int ctId);
        Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId);
        GolfCourseBO SelectCourseById(int id);
        CourseTermBO SelectTermById(int id);

        #endregion

        #region Equipment
        Task<List<EquipmentBO>> GetAllEquipment();
        Task<List<EquipmentTypesBO>> GetAllEquipmentTypes();
        List<EquipmentBO> GetSelEquipmentById(int[] selEqip);
        EquipmentBO SearchEquipment(int id);

        #endregion

        #region Rent
        List<RentBO> GetAllActiveRents();
        bool SaveRent(int ctId, int custId, DateTime rentDte);
        bool SaveRentItems(int ctId, int custId, int[] equipIds);

        #endregion

    }
}
