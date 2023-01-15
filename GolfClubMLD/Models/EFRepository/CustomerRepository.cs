using AutoMapper;
using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
            Task<List<CourseTermBO>> terms = _custEntities.CourseTerm.Select(ct => ct)
                                                .Where(ct => ct.courseId == courseId)
                                                .Include(ct => ct.GolfCourse)
                                                .Include(ct => ct.Term)
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
            DateTime test = tod.AddDays(dayNum - d);
        }

        public List<EquipmentBO> GetSelEquipmentById(int[] sel)
        {
            List<EquipmentBO> selectedEquip = new List<EquipmentBO>();
            EquipmentBO equip;
            for (int i = 0; i < sel.Length; i++)
            {
                equip = SearchEquipment(sel[i]);
                selectedEquip.Add(equip);
            }
            return selectedEquip;
        }
        private EquipmentBO SearchEquipment(int id)
        {
            Equipment eq = _custEntities.Equipment.FirstOrDefault(e => e.id == id);
            EquipmentBO equip = Mapper.Map<EquipmentBO>(eq);
            return equip;
        }

        public GolfCourseBO SelectCourseById(int id)
        {
            GolfCourse golfCour = _custEntities.GolfCourse.FirstOrDefault(gc => gc.id == id);
            GolfCourseBO selCourse = Mapper.Map<GolfCourseBO>(golfCour);
            return selCourse;
        }

        public CourseTermBO SelectTermById(int id)
        {
            CourseTerm cTerm = _custEntities.CourseTerm.FirstOrDefault(t => t.id == id);
            CourseTermBO selCTerm = Mapper.Map<CourseTermBO>(cTerm);
            return selCTerm;
        }
        public CustomerCreditCardViewModel GetCustomerCC(int id)
        {
            CustomerCreditCardViewModel custCC = new CustomerCreditCardViewModel();
            Users cust = _custEntities.Users.FirstOrDefault(u => u.id == id);
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            MailAddress ma_from = new MailAddress("golfClubMLDTest@gmail.com", "fromSomeone");
            MailAddress ma_to = new MailAddress("golfClubMLDTest@gmail.com", "ToSomeone");
            string s_password = "dqkoughastqgvqji";
            string s_subject = "Test";
            string s_body = "This is a Test";

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
                Port = 587,
                //enable SSL to be true, otherwise it will get kicked back by the Google server.
                EnableSsl = true,
                //The following properties need set as well
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ma_from.Address, s_password)
            };


            using (MailMessage mail = new MailMessage(ma_from, ma_to)
            {
                Subject = s_subject,
                Body = s_body

            })

                try
                {
                    Console.WriteLine("Sending Mail");
                    smtp.Send(mail);
                    Console.WriteLine("Mail Sent");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                    Console.ReadLine();
                }

        }
        public async Task<string> HashPassword(string pass)
        {
            MD5CryptoServiceProvider encryptor = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] encryptedValueBytes = encryptor.ComputeHash(encoder.GetBytes(pass));
            StringBuilder encryptedValueBuilder = new StringBuilder();
            for (int i = 0; i < encryptedValueBytes.Length; i++)
            {
                encryptedValueBuilder.Append(encryptedValueBytes[i].ToString("x2"));

            }
            string encryptedValue = encryptedValueBuilder.ToString();

            return encryptedValue;
        }
        public async Task<bool> EditCustomerData(CustomerCreditCardViewModel ccvm)
        {
            try
            {
                Users custForEdit = _custEntities.Users.FirstOrDefault(c=>c.id == ccvm.Cust.Id);
                CreditCard custCredCard = _custEntities.CreditCard.FirstOrDefault(c => c.id == ccvm.CredCard.Id);
                if (custForEdit == null || custCredCard == null)
                    return false;

                custForEdit.username = ccvm.Cust.Username;
                custForEdit.pass = await HashPassword(ccvm.Cust.Pass);
                custForEdit.fname = ccvm.Cust.Fname;
                custForEdit.lname = ccvm.Cust.Lname;
                custForEdit.phone = ccvm.Cust.Phone;
                custForEdit.profPic = ccvm.Cust.ProfPic;

                custCredCard.carNum = ccvm.CredCard.CarNum;
                custCredCard.own = ccvm.CredCard.Own;
                custCredCard.expiry = ccvm.CredCard.Expiry;
                custCredCard.cvv = ccvm.CredCard.CVV;

                _custEntities.Entry(custForEdit).State = EntityState.Modified;
                _custEntities.Entry(custCredCard).State = EntityState.Modified;
                _custEntities.SaveChanges();

            }
            catch(Exception ex)
            {

            }

            return true;
        }
    }
}