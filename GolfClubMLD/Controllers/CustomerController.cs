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
        public async Task<ActionResult> ReserveCourse(int courseId)
        {
            List<CourseTermBO> selCourseTerms = await _custRepo.GetTermsForSelCourse(courseId);
            if(selCourseTerms != null)
                return View(selCourseTerms);
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