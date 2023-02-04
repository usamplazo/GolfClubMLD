using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    [NoCache]
    [RoleAuthorize(Roles.Manager)]
    [HandleError(View = "Error", ExceptionType = typeof(Exception))]
    public class ManagerController : Controller
    {
        ManagerRepository _manrepo;
        public ManagerController()
        {
            _manrepo = new ManagerRepository();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> EditEquipment()
        {
            List<EquipmentBO> allEquip = await _manrepo.GetAllEquipment();
            if(allEquip != null)
                return View(allEquip);
            ViewBag.Message = "Nema dostupne opreme";
            return View();
        }
    }
}