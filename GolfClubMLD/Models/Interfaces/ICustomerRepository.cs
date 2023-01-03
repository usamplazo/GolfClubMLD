using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface ICustomerRepository
    {
        Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId);
        List<EquipmentBO> GetSelEquipmentById(int[] selEqip);
        CustomerCreditCardViewModel GetCustomerCC(int id);
        GolfCourseBO SelectCourseById(int id);
        CourseTermBO SelectTermById(int id);
        bool SaveRent(int ctId, int custId);
        bool SaveRentItems(int ctId, int custId, int[] equipIds);
        DateTime CalculateDate(DateTime now);
        void SendEmail();
    }
}
