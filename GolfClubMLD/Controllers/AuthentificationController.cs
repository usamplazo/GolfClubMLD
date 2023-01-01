using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GolfClubMLD.Controllers
{
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
        [MyExpirePage]
        [HttpPost]
        public async Task<ActionResult> Login(string email, string pass)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("email", "Morate uneti email");

                    return View();
                }
                if (string.IsNullOrEmpty(pass))
                {
                    ModelState.AddModelError("pass", "Morate uneti lozniku");
                }
                UsersBO customer = await _accRepo.LoginCustomer(email, pass);
                if (customer != null)
                {
                    FormsAuthentication.SetAuthCookie(customer.Username, true);
                    Session["LoginId"] = customer.Id.ToString();
                    Session["LoginEmail"] = customer.Email.ToString();
                    Session["LogCustCC"] = _accRepo.GetCredCardById(customer.CredCardId);
                    return RedirectToAction("Index","Home");
                    
                }
                ModelState.AddModelError("cust", "Ne posotji korisnik sa unetim podacima");

                return View(customer);
            }
            ModelState.AddModelError("cust", "Greska prilikom unosa");
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
                //return RedirectToActio);
            }
            //thow new Exception()
            return RedirectToAction("Index","Home");
        }
        [MyExpirePage]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}