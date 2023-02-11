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
        #region Customer

        UsersBO GetCustomerById(int custId);
        Task<CreditCardBO> GetCredCardById(int credCardId);
        CustomerCreditCardViewModel GetCustomerCCById(int id);

        #endregion

        #region CourseTerm

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
        List<RentBO> GetRentItems(List<RentBO> rents);
        bool SaveRent(int ctId, int custId, DateTime rentDte);
        bool SaveRentItems(int ctId, int custId, int[] equipIds);

        #endregion

    }
}
