using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private CustomerRepository _custRepo;
        public AdminController()
        {
            _adminRepo = new AdminRepository();
            _authRepo  = new AuthentificationRepository();
            _custRepo = new CustomerRepository();
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

            //get profile picture
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
        public ActionResult EditUser(int id)
        {
            UsersBO user = _adminRepo.GetUserById(id);
            UserAndCreditCardViewModel userData = new UserAndCreditCardViewModel()
            {
                Cust = user
            };

            if (user.RoleId == 1)
            {
                CreditCardBO cc = _adminRepo.GetCustomerCCById(user.Id);
                if(cc is null)
                {
                    ViewBag.AdminErrorMessage = "Ne postoje validni podaci za izmenu korisnika!";
                    return View("UserList", _adminRepo.GetAllUsers());
                }
                userData.CredCard = cc;
            }

            return View("EditUser", userData);
        }

        [HttpPost]
        public ActionResult EditUser(UserAndCreditCardViewModel userData)
        {
            if (!ModelState.IsValid)
                return View(userData);

            if (userData is null)
            {
                ViewBag.AdminErrorMessage = "Greska prilikom izmene korisnika (nema podataka za id: "+ userData.Cust.Id +")";
                return View("UserList", _adminRepo.GetAllUsers());
            }

            //get profile picture
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                userData.Cust.ProfPic = _adminRepo.GetImportedProfilePicture(file, Server.MapPath("~/Images/ProfileImages/"));
            }
            else
            {
                userData.Cust.ProfPic = string.Empty;
            }

            switch (userData.Cust.RoleId)
            {
                case 1: //Customer

                    if (userData.CredCard is null)
                    {
                        ViewBag.AdminErrorMessage = "Nema podataka o kreditnoj kartici!";
                        return View("UserList", _adminRepo.GetAllUsers());
                    }
                   
                    if (!_custRepo.EditCustomerData(userData))
                    {
                        ViewBag.AdminErrorMessage = "Azuriranje naloga korisnika nije uspelo!";
                        return View("UserList", _adminRepo.GetAllUsers());
                    }
                    break;

                case 2: //Manager

                    if (!_adminRepo.EditManagerData(userData.Cust))
                    {
                        ViewBag.AdminErrorMessage = "Azuriranje naloga menadzera nije uspelo!";
                        return View("UserList", _adminRepo.GetAllUsers());
                    }
                    break;
            }
            ViewBag.AdminMessage = "Azuriranje naloga uspesno obavljeno!";
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

        [HttpGet]
        public async Task<ActionResult> EquipmentList()
        {
            IEnumerable<EquipmentBO> allEquip = await _adminRepo.GetAllEquipment();
            return View(allEquip);
        }
        [HttpGet]
        public ActionResult EditEquipment(int id)
        {
            EquipmentBO equip = _adminRepo.SearchEquipment(id);
            return View(equip);
        }

        [HttpPost]
        public async Task<ActionResult> EditEquipment(EquipmentBO equip)
        {
            if (!ModelState.IsValid)
                return View(equip);

            //get profile picture
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                equip.PicUrl = _adminRepo.GetImportedProfilePicture(file, Server.MapPath("~/Images/EquipmentImages/"));
            }
            else
            {
                equip.PicUrl = string.Empty;
            }

            if(!_adminRepo.UpdateEquipment(equip))
            {
                ViewBag.AdminEquipmentUpdateError = "Greska prilikom izmene opreme id: " + equip.Id;
                return View();
            }
            ViewBag.AdminEquipmentUpdate = "Oprema uspesno izmenjena";
            return View(await _adminRepo.GetAllEquipment());
        }

        [HttpGet]
        public async Task<ActionResult> GolfCoursesList()
        {
            List<GolfCourseBO> courses = await _adminRepo.GetAllCourses();
            return View(courses);
        }

        [HttpGet]
        public async Task<ActionResult> EditCourse(int id)
        {
            GolfCourseBO course = await _adminRepo.GetCourseById(id);
            List<CourseTypeBO> ct = await _adminRepo.GetAllCourseTypes();
            ViewBag.CourseTypes = ct;
            return View(course);
        }

        [HttpPost]
        public async Task<ActionResult> EditCourse(GolfCourseBO course, int corTypId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseTypes = await _adminRepo.GetAllCourseTypes();
                return View(course);
            }
            //get course picture
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                course.PicUrl = _adminRepo.GetImportedProfilePicture(file, Server.MapPath("~/Images/CourseImages/"));
            }
            if (corTypId > 0)
            {
                course.CorTypId = corTypId;
            }
            if(!_adminRepo.EditCourse(course))
            {
                ViewBag.AdminCourseUpdateError = "Greska prilikom izmene teren id: " + course.Id;
                return View();
            }
            ViewBag.AdminCourseUpdate = "Uspesno izmenjen teren id: " + course.Id;
            return View("GolfCoursesList", await _adminRepo.GetAllCourses());
        }

        [HttpGet]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            GolfCourseBO gc = await _adminRepo.GetCourseById(id);

            return View(gc);
        }
    }
}