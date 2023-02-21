using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    [NoCache]
    [RoleAuthorize(Roles.Admin)]
    [HandleError(View = "Error", ExceptionType = typeof(Exception))]
    public class AdminController : Controller
    {
        private AdminRepository _adminRepo;
        public AdminController()
        {
            _adminRepo = new AdminRepository();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserList()
        {
            List<UsersBO> users = _adminRepo.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public ActionResult DeactUser(int id)
        {
            UsersBO userToDeactivate = _adminRepo.GetUserById(id);
            return View(userToDeactivate);
        }

        [HttpPost]
        public ActionResult Deactivate(int id)
        {
            if (_adminRepo.DeactCustomer(id))
            {
                ViewBag.AdminMessage = "Uspesno deaktiviran nalog korisnika sa id: " + id;

                return View("UserList", _adminRepo.GetAllUsers());
            }

            ViewBag.AdminErrorMessage = "Greska prilikom deaktivacije korisnika";
            return View();
        }
    }
}