using GolfClubMLD.Models;
using GolfClubMLD.Models.ActionFilters;
using GolfClubMLD.Models.Classes;
using GolfClubMLD.Models.EFRepository;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GolfClubMLD.Controllers
{
    [HandleError(View = "Error", ExceptionType = typeof(Exception))]
    [NoCache]
    public class CustomerController : Controller
    {
        private CustomerRepository _custRepo;
        public CustomerController()
        {
            _custRepo = new CustomerRepository();
        }
        [HttpGet]
        [RoleAuthorize(Roles.Customer)]
        public async Task<ActionResult> ReserveCourse(int courseId, string ctDay = null)
        {
            List<CourseTermBO> allCourseTerms;
            allCourseTerms = await _custRepo.GetTermsForSelCourse(courseId);

            if (string.IsNullOrWhiteSpace(ctDay))
                ctDay = DateTime.Now.Date.ToString();

            Session["selRentDate"] = ctDay;

            if (allCourseTerms != null)
            {
                List<string> avalDates = System.Web.Helpers.Json.Decode<List<string>>(Session["allDates"].ToString());
                Dictionary<string, List<CourseTermBO>> terms = new Dictionary<string, List<CourseTermBO>>();
                foreach (string ad in avalDates)
                {
                    List<CourseTermBO> selDayTerms = new List<CourseTermBO>();
                    foreach (CourseTermBO ct in allCourseTerms)
                    {
                        DateTime availableDate = DateTime.Parse(ad);
                        if (availableDate.DayOfWeek.ToString() == ct.dayOfW)
                        {
                            selDayTerms.Add(ct);
                        }
                    }
                    if (selDayTerms != null)
                    {
                        terms.Add(ad, selDayTerms);
                    }
                    //List<CourseTermBO> viewCT = await _custRepo.CheckForRentCourses(selDayTerms, courseId);
                }
                return View(terms);
            }
            ViewBag.Message = "Nemamo termina za selektovani teren ";
            return View();
        }
            [HttpPost]
            [RoleAuthorize(Roles.Customer)]
            public ActionResult ReserveCourse(int courseTermId)
            {
                Session["PickedCourseTermId"] = courseTermId;
                int dayNum = Convert.ToInt32(Session["DayNum"]);
                return RedirectToAction("HomeEquipment", "Home", courseTermId);
            }

            [HttpGet]
            public ActionResult PickTerm(DateTime ctDay)
            {

                return View();
            }

            [HttpPost]
            public ActionResult SelectedItem(int[] selItems)
            {
                int customerId = Convert.ToInt32(Session["LoginId"]);
                int courseTermId = Convert.ToInt32(Session["PickedCourseTermId"]);
                CustomerCreditCardViewModel custCC = _custRepo.GetCustomerCCById(customerId);
                CourseTermBO cTerm = _custRepo.SelectTermById(courseTermId);
                GolfCourseBO gc = cTerm.GolfCourse;
                List<EquipmentBO> selection = _custRepo.GetSelEquipmentById(selItems);
                if(selection is null)
                {
                    ViewBag.ErrorMessage = "Oprema nije selektovana";
                    return RedirectToAction("HomeEquipment","Home");
                }
                if (custCC == null || cTerm == null || gc == null)
                {
                     ViewBag.ErrorMessage = "Iznajmljivanje trenutno nije moguce";
                     return RedirectToAction("Index", "Error");
                }
                RentInfoConfirmViewModel info = new RentInfoConfirmViewModel() { 
                    CustomerCredCard = custCC,
                    Equipment = selection,
                    Course = gc,
                    CorTerm = cTerm
                };
                return View(info);
            }
            [HttpGet]
            public ActionResult Rent()
            {
                return View();
            }
            [HttpPost]
            public ActionResult Rent(RentInfoConfirmViewModel info, int[] equipIds)
            {
                DateTime rentDate = DateTime.Parse(Session["selRentDate"].ToString());
                if (_custRepo.SaveRent(info.CorTerm.Id, info.CustomerCredCard.Cust.Id, rentDate))
                {
                    _custRepo.SaveRentItems(info.CorTerm.Id, info.CustomerCredCard.Cust.Id, equipIds);
                    _custRepo.SendEmail();
                }
                return View();
            }
        [HttpGet]
        public ActionResult Edit(string custId)
        {
            if (int.TryParse(custId, out int id))
            {
                CustomerCreditCardViewModel custCC = _custRepo.GetCustomerCCById(id);
                return View(custCC);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(CustomerCreditCardViewModel ccvm)
        {
            if (ccvm == null)
                ViewBag.EditProfileErrorMessage = "Neispravno uneti podaci";

            if (ccvm.Cust.Pass == null)
                ViewBag.EditProfileErrorMessage = "Morate uneti i sifru za potvrdu";

            HttpPostedFileBase file = Request.Files["file"];
            if (file != null && file.ContentLength > 0)
            {
                // Save the file to a location on the server.
                string path = Path.Combine(Server.MapPath("~/Images/ProfileImages/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                ccvm.Cust.ProfPic = "/Images/ProfileImages/" + file.FileName;
            }
            if (_custRepo.EditCustomerData(ccvm))
            {
                ViewBag.EditProfileMessage = "Profile updated";
            }
            return View(_custRepo.GetCustomerCCById(Convert.ToInt32(Session["LoginId"])));
        }
        [HttpPost]
        [RoleAuthorize(Roles.Customer)]
        public ActionResult DeactivateProfile(int custId)
        {
            if (!_custRepo.DeactCustomer(custId))
                return View();
            ViewBag.Message = "Nalog deaktiviran";
            return RedirectToAction("Index", "Home");
        }

        }
    }