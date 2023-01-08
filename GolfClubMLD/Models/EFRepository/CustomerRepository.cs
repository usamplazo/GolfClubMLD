using AutoMapper;
using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.EFRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private GolfClubMldDBEntities _custEntities = new GolfClubMldDBEntities();
        private DateTime m_rentConf;
        private DateTime m_rentDate;
        private List<CourseTermBO> m_avlCourseTerms;
        public async Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId)
        {
            Task<List<CourseTermBO>> terms = _custEntities.CourseTerm.Select(ct=>ct)
                                                .Where(ct=>ct.courseId == courseId)
                                                .Include(ct => ct.GolfCourse)
                                                .Include(ct=>ct.Term)
                                                .ProjectTo<CourseTermBO>()
                                                .ToListAsync();
            return await CheckForRentCourses(await terms, courseId);
                                           
        }
        public async Task<List<CourseTermBO>> CheckForRentCourses(List<CourseTermBO> courseTerms, int courseId)
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddDays(7 - (int)start.DayOfWeek);
            var currentWeekRents = await _custEntities.Rent.Select(r => r)
                                                            .Where(r => r.CourseTerm.courseId == courseId && (r.rentDate >= start.Date && r.rentDate < end.Date))
                                                            .ToListAsync();

            List<CourseTermBO> corTrm = new List<CourseTermBO>(courseTerms);
            if (currentWeekRents != null)
            {
                foreach (CourseTermBO ct in courseTerms)
                {
                    foreach (Rent r in currentWeekRents)
                    {
                        if (r.courTrmId == ct.Id)
                            corTrm.Remove(ct);
                    }
                }
            }
            m_avlCourseTerms = corTrm;
            return corTrm;
            
        }
        public void CalculateDateFromDayNumber(int dayNum)
        {
            var tod = DateTime.Now;
            var d = (int)tod.DayOfWeek;
            DateTime test = tod.AddDays(dayNum-d);
        }

        public List<EquipmentBO> GetSelEquipmentById(int[] sel)
        {
            List<EquipmentBO> selectedEquip = new List<EquipmentBO>();
            EquipmentBO equip;
            for(int i = 0; i < sel.Length; i++)
            {
                equip =  SearchEquipment(sel[i]);
                selectedEquip.Add(equip);
            }
            return selectedEquip;
        }
        private EquipmentBO SearchEquipment(int id)
        {
            Equipment eq =  _custEntities.Equipment.FirstOrDefault(e => e.id == id);
            EquipmentBO equip = Mapper.Map<EquipmentBO>(eq);
            return equip;
        }

        public GolfCourseBO SelectCourseById(int id)
        {
            GolfCourse golfCour = _custEntities.GolfCourse.FirstOrDefault(gc=>gc.id == id);
            GolfCourseBO selCourse = Mapper.Map<GolfCourseBO>(golfCour);
            return  selCourse;
        }

        public CourseTermBO SelectTermById(int id)
        { 
            CourseTerm cTerm = _custEntities.CourseTerm.FirstOrDefault(t=>t.id == id);
            CourseTermBO selCTerm = Mapper.Map<CourseTermBO>(cTerm);
            return selCTerm; 
        }
        public CustomerCreditCardViewModel GetCustomerCC(int id)
        {
            CustomerCreditCardViewModel custCC = new CustomerCreditCardViewModel();
            Users cust = _custEntities.Users.FirstOrDefault(u=>u.id == id);
            UsersBO logedCust = Mapper.Map<UsersBO>(cust);
            CreditCardBO cc = logedCust.CreditCard;
            custCC.Cust = logedCust;
            custCC.CredCard = cc;

            return custCC;
        }
        public bool SaveRent(int ctId, int custId, DateTime rentDte)
        {
            try
            {
                m_rentConf = DateTime.Now;
                CourseTermBO selCt = SelectCourseTermById(ctId);
                Rent rentInfo = new Rent()
                {
                    billDate = m_rentConf,
                    totPrice = 1000,
                    courTrmId = ctId,
                    custId = custId,
                    rentDate = rentDte
                };
                _custEntities.Rent.Add(rentInfo);
                _custEntities.SaveChanges();
            }
            catch(Exception ex)
            {

            }    
            return true;
        }

        public CourseTermBO SelectCourseTermById(int ctId)
        {
            CourseTerm ct = _custEntities.CourseTerm.FirstOrDefault(c => c.id == ctId);
            CourseTermBO courTerm = Mapper.Map<CourseTermBO>(ct);

            return courTerm;
        }

        public bool SaveRentItems(int ctId, int custId, int[] equipIds)
        {
            try
            {
                Rent rent = _custEntities.Rent.FirstOrDefault(r => r.custId == custId 
                                                                && r.courTrmId == ctId
                                                                && r.billDate == m_rentConf);
                List<EquipmentBO> equip = GetSelEquipmentById(equipIds);
                if (rent != null) {
                    RentItems re = new RentItems();
                    foreach (var eq in equip)
                    {
                        re.equipId = eq.Id;
                        re.price = eq.Price;
                    }
                    _custEntities.RentItems.Add(re);
                    _custEntities.SaveChanges();
                }
            }
            catch(Exception ex)
            {

            }
            return true;
        }
        public DateTime CalculateDate(DateTime now)
        {
            string[] st = new string[] { "one", "two" };
            st.AsEnumerable<string>().Select(x => DateTime.Now.ToString());
            int delta = (DayOfWeek.Monday - now.DayOfWeek - 7) % 7;
            DateTime dt = now.AddDays(delta);
            return dt;
        }
        public void SendEmail()
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("GolfClubMLD", "uzumakis880@gmail.com"));
            mailMessage.To.Add(new MailboxAddress("test user", "uzumakis880@gmail.com"));
            mailMessage.Subject = "Test mail";
            mailMessage.Body = new TextPart()
            {
                Text = "Test mail"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, false);
                //smtpClient.Authenticate("user", "password");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}