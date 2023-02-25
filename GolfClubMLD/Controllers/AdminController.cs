using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    [NoCache]
    [RoleAuthorize(Roles.Admin)]
    [HandleError(View = "Error", ExceptionType = typeof(Exception))]
    public class AdminController : Controller
    {
        private AdminRepository _adminRepo;
        private AuthentificationRepository _authRepo;
        public AdminController()
        {
            _adminRepo = new AdminRepository();
            _authRepo  = new AuthentificationRepository();
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
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(UserAndCreditCardViewModel custCredCard)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
                custCredCard.Cust.ProfPic = _adminRepo.GetImportedProfilePicture(file, Server.MapPath("~/Images/ProfileImages/"));
            else
                custCredCard.Cust.ProfPic = string.Empty;
         
            bool createdCust = _authRepo.RegisterCustomer(custCredCard);

            if (!createdCust)
            {
                ViewBag.AdminErrorMessage = "Registracija korisnika nije uspela !";
            }
            return View("UserList", _adminRepo.GetAllUsers());
        }

        [HttpGet]
        public ActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateManager(UsersBO manag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string hashPass = _adminRepo.HashPassword(manag.Pass);
            if(string.IsNullOrWhiteSpace(hashPass))
            {
                //
                return View();
            }
            manag.Pass = hashPass;

            //get profile pic
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                manag.ProfPic = _adminRepo.GetImportedProfilePicture(file, Server.MapPath("~/Images/ProfileImages/"));
            }
            else
            {
                manag.ProfPic = string.Empty;
            }

            manag.RoleId = 2;
            if (!_adminRepo.RegisterManager(manag))
            {
                ViewBag.AdminErrorMessage = "Registracija menadzera nije uspela !";
                return View();
            }

            return View("UserList", _adminRepo.GetAllUsers());
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