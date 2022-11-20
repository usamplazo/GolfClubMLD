using GolfClubMLD.Models;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    public class HomeController : Controller
    {
        private IHomeRepository _homeRepo;
        public HomeController()
        {
            _homeRepo = new HomeRepository();
        }
        public ActionResult Index()
        {
            IEnumerable<GolfCourseBO> allCourses = _homeRepo.GetAllCourses();
            return View(allCourses);
        }

        public ActionResult About(int id)
        {
            //ViewBag.Message = "Your application description page.";
            GolfCourseBO course = _homeRepo.GetCourseById(id);
            return View(course);
        }
        public ActionResult SearchCourse(string search)
        {
            IEnumerable<GolfCourseBO> searchRes = _homeRepo.GetCoursesBySearch(search);
            return View(searchRes);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}