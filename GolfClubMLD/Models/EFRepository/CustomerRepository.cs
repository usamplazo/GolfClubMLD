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
        public async Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId)
        {
            Task<List<CourseTermBO>> terms = _custEntities.CourseTerm.Select(ct=>ct)
                                                .Where(ct=>ct.courseId == courseId)
                                                .Include(ct => ct.GolfCourse)
                                                .Include(ct=>ct.Term)
                                                .ProjectTo<CourseTermBO>()
                                                .ToListAsync();
            CheckForRentCourses(await terms);
            return await terms;                                  
        }
        public void CheckForRentCourses(List<CourseTermBO> courseTermBOs)
        {
            List<RentBO> currentWeekRents = _custEntities.Rent.Select(r=>r)
                                                .Where(r=>r.)
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
        public bool SaveRent(int ctId, int custId)
        {
            try
            {
                m_rentConf = DateTime.Now;
                Rent rentInfo = new Rent()
                {
                    billDate = m_rentConf,
                    totPrice = 1000,
                    courTrmId = ctId,
                    custId = custId
                };
                _custEntities.Rent.Add(rentInfo);
                _custEntities.SaveChanges();
            }
            catch(Exception ex)
            {

            }    
            return true;
        }

        public bool SaveRentItems(int ctId, int custId, int[] equipIds)
        {
            try
            {
                Rent rent = _custEntities.Rent.FirstOrDefault(r => r.courTrmId == ctId
                                                                && r.custId == custId
                                                                && r.billDate == m_rentConf);
                List<EquipmentBO> equip = GetSelEquipmentById(equipIds);
                DateTime rentDt = CalculateDate(DateTime.Now);
                if (rent != null) {
                    RentItems re = new RentItems();
                    foreach (var eq in equip)
                    {
                        re.equipId = eq.Id;
                        re.price = eq.Price;
                        re.rentDate = m_rentConf;
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