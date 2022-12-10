using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
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
        [HttpGet]
        [MyExpirePage]
        public async Task<ViewResult> Index(string searchString, int? page)
        {
            List<GolfCourseBO> allCourses = await _homeRepo.GetAllCourses();
            List<CourseTypeBO> allTypes = await _homeRepo.GetAllCourseTypes();
            ViewData["types"] = allTypes.Select(t => t).ToList<CourseTypeBO>();
            if (allCourses == null)
                return View();

            if (Int32.TryParse(searchString, out int typeId))
            {
                allCourses = await _homeRepo.GetCoursesByType(typeId);
                page = 1;
                if (allCourses.Count == 0)
                    return View();
                return View(allCourses);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allCourses = await _homeRepo.GetCoursesBySearch(searchString);
                page = 1;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(allCourses.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> HomeEquipment(int typeId = 0)
        {

            List<EquipmentBO> allEquip = await _homeRepo.GetAllEquipment();
            List<EquipmentTypesBO> allTypes = await _homeRepo.GetAllEquipmentTypes();
            ViewData["equipTypes"] = allTypes.Select(et => et).ToList<EquipmentTypesBO>();

            if (allEquip == null)
                return View();

            if (typeId > 0)
            {
                allEquip = await _homeRepo.GetEquipmentByType(typeId);
                if (allEquip.Count == 0)
                    return View();
                return View(allEquip);
            }
            return View(allEquip);
        }
        public async Task<ActionResult> Details(int id)
        {
            GolfCourseBO gc = await _homeRepo.GetCourseById(id);
            return View(gc);
        }
    }
}