using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public async Task<ActionResult> EquipmentList()
        {
            List<EquipmentBO> allEquip = await _manrepo.GetAllEquipment();
            if (allEquip != null)
                return View(allEquip);
           
            ViewBag.Message = "Nema dostupne opreme";
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> EditEquipment(int id)
        {
            EquipmentBO equip = _manrepo.SearchEquipment(id);
            IEnumerable<EquipmentTypesBO> allEquipTypes = await _manrepo.GetAllEquipmentTypes();
            if (equip != null && allEquipTypes != null)
            {
                ViewBag.AllEquipTypes = allEquipTypes;
                return View(equip);
            }
            ViewBag.ErrorMessage = "Nema dostupne opreme";
            return RedirectToAction("Index", "Error");
        }
        [HttpPost]
        public ActionResult EditEquipment(EquipmentBO equipToEdit, int eqTypId = 0)
        {
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null && file.ContentLength > 0)
            {
                // Save the file to a location on the server.
                string path = Path.Combine(Server.MapPath("~/Images/EquipmentImages/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                equipToEdit.PicUrl = "/Images/EquipmentImages/" + file.FileName;
            }
            if(eqTypId <= 0)
            {
                //default equipId
                eqTypId = 100;
            }
            equipToEdit.EquipmentTypId = eqTypId;
            if (!_manrepo.SaveEditedEquipment(equipToEdit))
            {
                ViewBag.ErrorMessage = "Greska prilikom editovanja selektovane opreme";
                return RedirectToAction("Index", "Error");
            }
            ViewBag.Message = "Oprema uspesno izmenjena";
            return RedirectToAction("EquipmentList", "Manager");
        }
        [HttpGet]
        public ActionResult RentList()
        {
            List<RentBO> activeRents = _manrepo.GetAllActiveRents();
            return View(activeRents);
        }

    }
}