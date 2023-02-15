using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace GolfClubMLD.Controllers
{
    [HandleError(View = "Error", ExceptionType = typeof(Exception))]
    [NoCache]
    public class AuthentificationController : Controller
    {

        private AuthentificationRepository _accRepo;
        public AuthentificationController()
        {
            _accRepo = new AuthentificationRepository();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        //public async Task<ActionResult> Login(string email, string pass)
        public async Task<ActionResult> Login(UserLoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                UsersBO user = await _accRepo.LoginUser(loginUser.Email, loginUser.Pass);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, true);
                    Session["LoginId"] = user.Id.ToString();
                    Session["LoginEmail"] = user.Email.ToString();
                    Session["LogCustCC"] = _accRepo.GetCredCardById(user.CredCardId);
                    string controllerName = user.Role.Name;
                    if (controllerName.Equals("Customer"))
                        return RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", controllerName);

                       
                }
                ViewBag.ErrorMessage = "Ne posotji korisnik sa unetim podacima";
                return View();
            }
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(UserAndCreditCardViewModel custCredCard)
        {
            if (ModelState.IsValid)
            {
                bool areSaved = await _accRepo.RegisterCustomer(custCredCard);
                if (areSaved)
                    return RedirectToAction("Index", "Home");
                else
                    ViewBag.ErrorMessage = "Registracija nije uspela !";
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}