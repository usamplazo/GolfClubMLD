using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using GolfClubMLD.Controllers;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Validation;

namespace GolfClubMLD.Models.EFRepository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        private GolfClubMldDBEntities _custEntities = new GolfClubMldDBEntities();
        private readonly ILogger<CustomerController> _logger;
        private DateTime m_rentConf;
        public CustomerRepository(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }
        public CustomerRepository()
        {

        }

        public DateTime CalculateDate(DateTime now)
        {
            string[] st = new string[] { "one", "two" };
            st.AsEnumerable<string>().Select(x => DateTime.Now.ToString());
            int delta = (DayOfWeek.Monday - now.DayOfWeek - 7) % 7;
            DateTime dt = now.AddDays(delta);
            return dt;
        }

        public void SendEmail(string emailTo, string name)
        {
            MailAddress maFrom = new MailAddress("golfClubMLDTest@gmail.com", "fromSomeone");
            MailAddress maTo = new MailAddress("golfClubMLDTest@gmail.com", "ToSomeone");
            //MailAddress maTo = new MailAddress(emailTo, name);

            string sPassword = "dqkoughastqgvqji";
            string sSubject = "Test";
            string Sbody = "This is a Test";

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(maFrom.Address, sPassword)
            };


            using (MailMessage mail = new MailMessage(maFrom, maTo)
            {
                Subject = sSubject,
                Body = Sbody

            })

                try
                {
                    smtp.Send(mail);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy ||
                            status == SmtpStatusCode.MailboxUnavailable)
                        {
                            _logger.LogError("Delivery failed - retrying in 5 seconds.");
                            System.Threading.Thread.Sleep(5000);
                            smtp.Send(mail);
                        }
                        else
                        {
                            _logger.LogError("Failed to deliver message to {0}",
                            ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                }
        }

        public bool EditCustomerData(UserAndCreditCardViewModel ccvm)
        {
            try
            {
                Users custForEdit = _custEntities.Users.FirstOrDefault(c=>c.id == ccvm.Cust.Id);
                CreditCard custCredCard = _custEntities.CreditCard.FirstOrDefault(c => c.id == ccvm.CredCard.Id);
                if (custForEdit == null || custCredCard == null)
                    return false;

                custForEdit.username = ccvm.Cust.Username;
                custForEdit.pass =  HashPassword(ccvm.Cust.Pass);
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
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    _logger.LogError("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        _logger.LogError("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: Customer => EditCustomerData " + ex);
            }
            return true;
        }


    }
}