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
            List<CourseTypeBO> allTypes = await _homeRepo.GetAllCourseTypes();

            ViewData["types"] = allTypes.Select(t => t).ToList<CourseTypeBO>();
            if (allCourses == null)
                return View();

            if(Int32.TryParse(searchString, out int typeId))
            {
                allCourses = await _homeRepo.GetCoursesByType(typeId);
                if (allCourses.Count == 0)
                    return View();
                return View(allCourses);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allCourses = await _homeRepo.GetCoursesBySearch(searchString);
            }
            return View(allCourses);
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
       
    }
}