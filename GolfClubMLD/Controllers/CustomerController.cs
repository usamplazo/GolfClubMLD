using GolfClubMLD.Models;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerRepository _custRepo;
        private Dictionary<int, DayOfWeek> m_daysOfWeek = new Dictionary<int, DayOfWeek>();
        public CustomerController()
        {
            _custRepo = new CustomerRepository();
            m_daysOfWeek.Add(0, DayOfWeek.Sunday);
            m_daysOfWeek.Add(1, DayOfWeek.Monday);
            m_daysOfWeek.Add(2, DayOfWeek.Tuesday);
            m_daysOfWeek.Add(3, DayOfWeek.Wednesday);
            m_daysOfWeek.Add(4, DayOfWeek.Thursday);
            m_daysOfWeek.Add(5, DayOfWeek.Friday);
            m_daysOfWeek.Add(6, DayOfWeek.Saturday);
        }
        [HttpGet]
        [RoleAuthorize(Roles.Customer)]
        public async Task<ActionResult> ReserveCourse(int courseId, int selDay=1)
        {
            List<CourseTermBO> allCourseTerms;
            allCourseTerms = await _custRepo.GetTermsForSelCourse(courseId);
            ViewData["CourseId"] = courseId;
            if (allCourseTerms != null)
            {
                ViewData["DaysOfWeek"] = m_daysOfWeek;
                ViewData["SelDay"] = m_daysOfWeek[selDay];
                List<CourseTermBO> selDayTerms = new List<CourseTermBO>();
                    foreach (var ct in allCourseTerms)
                    {
                    if (ct.dayOfW == m_daysOfWeek[selDay].ToString())
                        selDayTerms.Add(ct);
                    }
                return View(selDayTerms);
            }
            //Error handeling (nemamo termina za teren)
            return View();
        }
        [HttpPost]
        [RoleAuthorize(Roles.Customer)]
        public ActionResult ReserveCourse(string courseTerm)
        {
            Session["PickedCourseTermId"] = courseTerm;
            return RedirectToAction("HomeEquipment", "Home", courseTerm);
        }
        [HttpPost]
        public ActionResult SelectedItem(int[] selItems)
        {
            int customerId = Convert.ToInt32(Session["LoginId"]);
            int courseTermId = Convert.ToInt32(Session["PickedCourseTermId"]);
            CustomerCreditCardViewModel custCC = _custRepo.GetCustomerCC(customerId);
            CourseTermBO Cterm = _custRepo.SelectTermById(courseTermId);
            GolfCourseBO gc = Cterm.GolfCourse;
            List<EquipmentBO> selection = _custRepo.GetSelEquipmentById(selItems);
            RentInfoConfirmViewModel info = new RentInfoConfirmViewModel();
            info.CustomerCredCard = custCC;
            info.Equipment = selection;
            info.Course = gc;
            info.CorTerm = Cterm;
            return View(info);
        }
        [HttpGet]
        public ActionResult Rent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Rent(RentInfoConfirmViewModel info, int[] equipIds)
        {
            if (_custRepo.SaveRent(info.CorTerm.Id, info.CustomerCredCard.Cust.Id))
            {
                _custRepo.SaveRentItems(info.CorTerm.Id, info.CustomerCredCard.Cust.Id, equipIds);
                _custRepo.SendEmail();
            }
            return View();
        }
    }
}