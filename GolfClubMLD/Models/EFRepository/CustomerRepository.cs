using AutoMapper;
using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GolfClubMLD.Models.EFRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private GolfClubMldDBEntities _custEntities = new GolfClubMldDBEntities();
        public async Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId)
        {
            Task<List<CourseTermBO>> terms = _custEntities.CourseTerm.Select(ct=>ct)
                                                .Where(ct=>ct.courseId == courseId)
                                                .Include(ct => ct.GolfCourse)
                                                .Include(ct=>ct.Term)
                                                .ProjectTo<CourseTermBO>()
                                                .ToListAsync();

            return await terms;                                  
        }
       public void ConfirmRentInfo(RentInfoConfirmViewModel conf)
        {

        }
        public List<EquipmentBO> GetSelEquipmentById(int[] sel)
        {
            List<EquipmentBO> selectedEquip = new List<EquipmentBO>();
            EquipmentBO equip;
            for(int i = 0; i < sel.Length; i++)
            {
                equip =  SearchEquipment(sel[i]);
                selectedEquip.Add(equip);
            }
            return selectedEquip;
        }
        private EquipmentBO SearchEquipment(int id)
        {
            Equipment eq =  _custEntities.Equipment.FirstOrDefault(e => e.id == id);
            EquipmentBO equip = Mapper.Map<EquipmentBO>(eq);
            return equip;
        }

        public GolfCourseBO SelectCourseById(int id)
        {
            GolfCourse golfCour = _custEntities.GolfCourse.FirstOrDefault(gc=>gc.id == id);
            GolfCourseBO selCourse = Mapper.Map<GolfCourseBO>(golfCour);
            return  selCourse;
        }

        public CourseTermBO SelectTermById(int id)
        { 
            CourseTerm cTerm = _custEntities.CourseTerm.FirstOrDefault(t=>t.id == id);
            CourseTermBO selCTerm = Mapper.Map<CourseTermBO>(cTerm);
            return selCTerm; 
        }
        public CustomerCreditCardViewModel GetCustomerCC(int id)
        {
            CustomerCreditCardViewModel custCC = new CustomerCreditCardViewModel();
            Users cust = _custEntities.Users.FirstOrDefault(u=>u.id == id);
            UsersBO logedCust = Mapper.Map<UsersBO>(cust);
            CreditCardBO cc = logedCust.CreditCard;
            custCC.Cust = logedCust;
            custCC.CredCard = cc;

            return custCC;
        }
    }
}