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
            if (file != null && file.ContentLength > 0)
            {
                // Save the file to a location on the server.
                string path = Path.Combine(Server.MapPath("~/Images/ProfileImages/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                custCredCard.Cust.ProfPic = "/Images/ProfileImages/" + file.FileName;
            }
            bool createdCust = _authRepo.RegisterCustomer(custCredCard);

            if (!createdCust)
            {
                ViewBag.ErrorMessage = "Registracija nije uspela !";
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