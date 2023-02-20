using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    [HandleError(View = "Error", ExceptionType = typeof(Exception))]
    [NoCache]
    public class HomeController : Controller
    {
        private HomeRepository _homeRepo;
        public HomeController()
        {
            _homeRepo = new HomeRepository();
        }

        [HttpGet]
        public async Task<ActionResult> Index(string searchString, int? page, string order = null)
        {
            List<GolfCourseBO> allCourses = await _homeRepo.GetAllCourses();
            List<CourseTypeBO> allTypes   = await _homeRepo.GetAllCourseTypes();
            ViewData["types"] = allTypes.Select(t => t).ToList();
            ViewData["searchString"] = searchString;
            ViewData["order"] = order;
            if (allCourses is null)
            {
                ViewBag.Message = "Trenutno nema terena za iznajmljivanje";
                return View();
            }
            if (allTypes is null)
                return View();

            if (searchString != null && Int32.TryParse(searchString, out int typeSearchId))
            {
                allCourses = await _homeRepo.GetCoursesByType(typeSearchId);
                ViewData["CourseTypeName"] = allCourses[0].CourseType.Name;
                page = page ?? 1;
                if (allCourses.Count == 0)
                    return View();
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                allCourses = await _homeRepo.GetCoursesBySearch(searchString);
                page = page ?? 1;
            }

            switch (order)
            {
                case "asc":
                    allCourses = allCourses.OrderBy(c => c.Price).ToList();
                    break;
                case "desc":
                    allCourses = allCourses.OrderByDescending(c => c.Price).ToList();
                    break;
                default:
                    allCourses = allCourses.OrderBy(c=>c.Name).ToList();
                    break;
            };
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(allCourses.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            GolfCourseBO gc = await _homeRepo.GetCourseById(id);
            if (gc is null)
                return View();

            Session["selCourse"] = id;

            return View(gc);
        }

        [HttpGet]
        public async Task<ActionResult> HomeEquipment(int typeId = 0)
        {
            List<EquipmentBO> allEquip = await _homeRepo.GetAllEquipment();
            List<EquipmentTypesBO> allTypes = await _homeRepo.GetAllEquipmentTypes();
            ViewData["equipTypes"] = allTypes.Select(et => et).ToList();

            if (allEquip is null)
                return View();

            if (typeId > 0)
            {
                allEquip = await _homeRepo.GetEquipmentByType(typeId);
                if (allEquip.Count == 0)
                    return View();
            }
            return View(allEquip);
        }

    }
}