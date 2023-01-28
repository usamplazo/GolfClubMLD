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

        private IAuthentificationRepository _accRepo;
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
                UsersBO customer = await _accRepo.LoginCustomer(loginUser.Email, loginUser.Pass);
                if (customer != null)
                {
                    FormsAuthentication.SetAuthCookie(customer.Username, true);
                    Session["LoginId"] = customer.Id.ToString();
                    Session["LoginEmail"] = customer.Email.ToString();
                    Session["LogCustCC"] = _accRepo.GetCredCardById(customer.CredCardId);
                    return RedirectToAction("Index", "Home");
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
        public async Task<ActionResult> Register(CustomerCreditCardViewModel custCredCard)
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