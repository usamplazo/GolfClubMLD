using GolfClubMLD.Models;
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
    public class HomeController : Controller
    {
        private IHomeRepository _homeRepo;
        public HomeController()
        {
            _homeRepo = new HomeRepository();
        }
        public async Task<ActionResult> Index(string searchString)
        {
            List<GolfCourseBO> allCourses = await _homeRepo.GetAllCourses();

            if (allCourses == null)
                return View();

            if (!String.IsNullOrEmpty(searchString))
            {
                allCourses =await _homeRepo.GetCoursesBySearch(searchString);
            }
            return View(allCourses);
            }

            public ActionResult About(int id)
        {
            //ViewBag.Message = "Your application description page.";
            Task<GolfCourseBO> course = _homeRepo.GetCourseById(id);
            return View(course);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}