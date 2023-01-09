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
        private Dictionary<int, string> m_daysOfWeek = new Dictionary<int, string>();
        public CustomerController()
        {
            _custRepo = new CustomerRepository();
            m_daysOfWeek.Add(1, "Monday");
            m_daysOfWeek.Add(2, "Tuesday");
            m_daysOfWeek.Add(3, "Wednesday");
            m_daysOfWeek.Add(4, "Thursday");
            m_daysOfWeek.Add(5, "Friday");
            m_daysOfWeek.Add(6, "Saturday");
            m_daysOfWeek.Add(7, "Sunday");

        }
        [HttpGet]
        [RoleAuthorize(Roles.Customer)]
        public async Task<ActionResult> ReserveCourse(int courseId, string ctDay = null)
        {
            List<CourseTermBO> allCourseTerms;
            allCourseTerms = await _custRepo.GetTermsForSelCourse(courseId);
            if (string.IsNullOrWhiteSpace(ctDay))
                ctDay = DateTime.Now.Date.ToString();
            Session["selRentDate"] = ctDay;
            if (allCourseTerms != null)
            {
                List<string> avalDates = System.Web.Helpers.Json.Decode<List<string>>(Session["allDates"].ToString());
                Dictionary<string, List<CourseTermBO>> terms = new Dictionary<string, List<CourseTermBO>>();
                foreach (string ad in avalDates)
                {
                    List<CourseTermBO> selDayTerms = new List<CourseTermBO>();
                    foreach (CourseTermBO ct in allCourseTerms)
                    {
                        DateTime availableDate = DateTime.Parse(ad);
                        if (availableDate.DayOfWeek.ToString() == ct.dayOfW)
                        {
                            selDayTerms.Add(ct);
                        }
                    }
                    if (selDayTerms != null)
                    {
                        terms.Add(ad, selDayTerms);
                    }
                    //List<CourseTermBO> viewCT = await _custRepo.CheckForRentCourses(selDayTerms, courseId);
                }
                return View(terms);
            }
                //Error handeling (nemamo termina za teren)
                return View();
        }
            [HttpPost]
            [RoleAuthorize(Roles.Customer)]
            public ActionResult ReserveCourse(int courseTermId)
            {
                Session["PickedCourseTermId"] = courseTermId;
                int dayNum = Convert.ToInt32(Session["DayNum"]);
                return RedirectToAction("HomeEquipment", "Home", courseTermId);
            }

            [HttpGet]
            public ActionResult PickTerm(DateTime ctDay)
            {

                return View();
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
                RentInfoConfirmViewModel info = new RentInfoConfirmViewModel() { 
                    CustomerCredCard = custCC,
                    Equipment = selection,
                    Course = gc,
                    CorTerm = Cterm
                };
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
                DateTime rentDate = DateTime.Parse(Session["selRentDate"].ToString());
                if (_custRepo.SaveRent(info.CorTerm.Id, info.CustomerCredCard.Cust.Id, rentDate))
                {
                    _custRepo.SaveRentItems(info.CorTerm.Id, info.CustomerCredCard.Cust.Id, equipIds);
                    _custRepo.SendEmail();
                }
                return View();
            }
        }
    }