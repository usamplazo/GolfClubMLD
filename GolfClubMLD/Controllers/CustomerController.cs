using GolfClubMLD.Models;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
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
        public CustomerController()
        {
            _custRepo = new CustomerRepository();
        }
        [HttpGet]
        [RoleAuthorize(Roles.Customer)]
        public async Task<ActionResult> ReserveCourse(int courseId, string selDay)
        {
            List<CourseTermBO> allCourseTerms;
            allCourseTerms = await _custRepo.GetTermsForSelCourse(courseId);
            ViewData["CourseId"] = courseId;
            if (allCourseTerms != null)
            {
                string[] daysOfWeek = new string[7] { "Mon", "Tue", "Wen", "Thu", "Fri", "Sat", "Sun" };
                ViewData["DaysOfWeek"] = daysOfWeek;
                if (string.IsNullOrEmpty(selDay))
                    selDay = "Mon";
                ViewData["SelDay"] = selDay;
                List<CourseTermBO> selDayTerms = new List<CourseTermBO>();
                    foreach (var ct in allCourseTerms)
                    {
                    if (ct.dayOfW == selDay)
                        selDayTerms.Add(ct);
                    }
                return View(selDayTerms);

            }
            //Error handeling (nemamo termina za teren)
            return View();
        }
        [HttpPost]
        [RoleAuthorize(Roles.Customer)]
        public async Task<ActionResult> ReserveCourse()
        {
            return View();
        }
    }
}