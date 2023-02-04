﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using GolfClubMLD.Controllers;
using GolfClubMLD.Models.Interfaces;
using GolfClubMLD.Models.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.EFRepository
{
    public class BaseRepository : IBaseRepository
    {
        private GolfClubMldDBEntities _baseEntities = new GolfClubMldDBEntities();
        private readonly ILogger<CustomerController> _logger;
        private DateTime m_rentConf;
        public BaseRepository(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }
        public BaseRepository()
        {

        }
        public UsersBO GetCustomerById(int custId)
        {
            Users customer = _baseEntities.Users.FirstOrDefault(c => c.id == custId);
            if (customer == null)
                return null;
            UsersBO custBO = Mapper.Map<UsersBO>(customer);

            return custBO;
        }
        public async Task<CreditCardBO> GetCredCardById(int credCardId)
        {
            CreditCard credCard = await _baseEntities.CreditCard.FirstOrDefaultAsync(cc => cc.id == credCardId);
            CreditCardBO crCd = Mapper.Map<CreditCardBO>(credCard);
            return crCd;
        }
        public CustomerCreditCardViewModel GetCustomerCCById(int id)
        {
            CustomerCreditCardViewModel custCC = new CustomerCreditCardViewModel();
            Users cust = _baseEntities.Users.FirstOrDefault(u => u.id == id);
            UsersBO logedCust = Mapper.Map<UsersBO>(cust);
            CreditCardBO cc = logedCust.CreditCard;
            custCC.Cust = logedCust;
            custCC.CredCard = cc;

            return custCC;
        }
        public async Task<List<CourseTermBO>> CheckForRentCourses(List<CourseTermBO> courseTerms, int courseId)
        {
            try
            {
                DateTime start = DateTime.Now;
                DateTime end = start.AddDays(7 - (int)start.DayOfWeek);
                var currentWeekRents = await _baseEntities.Rent.Select(r => r)
                                                                .Where(r => r.CourseTerm.courseId == courseId
                                                                        && (r.rentDate >= start.Date
                                                                        && r.rentDate < end.Date))
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
                return corTrm;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, " Error: CustomerRepository / CheckForRentCourses => currentWeekRents ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error: CustomerRepository => CheckForRentCourses ");
            }
            return null;
        }
        public CourseTermBO SelectCourseTermById(int ctId)
        {
            CourseTerm ct = _baseEntities.CourseTerm.FirstOrDefault(c => c.id == ctId);
            CourseTermBO courTerm = Mapper.Map<CourseTermBO>(ct);

            return courTerm;
        }
        public async Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId)
        {
            Task<List<CourseTermBO>> terms = _baseEntities.CourseTerm.Select(ct => ct)
                                                .Where(ct => ct.courseId == courseId)
                                                .Include(ct => ct.GolfCourse)
                                                .Include(ct => ct.Term)
                                                .ProjectTo<CourseTermBO>()
                                                .ToListAsync();
            return await CheckForRentCourses(await terms, courseId);

        }
        public async Task<List<EquipmentBO>> GetAllEquipment()
        {
            Task<List<EquipmentBO>> allEquip = _baseEntities.Equipment
                 .Select(e => e)
                 .Include(et => et.EquipmentTypes)
                 .ProjectTo<EquipmentBO>()
                 .ToListAsync();

            return await allEquip;
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
            Equipment eq = _baseEntities.Equipment.FirstOrDefault(e => e.id == id);
            EquipmentBO equip = Mapper.Map<EquipmentBO>(eq);
            return equip;
        }
        public GolfCourseBO SelectCourseById(int id)
        {
            GolfCourse golfCour = _baseEntities.GolfCourse.FirstOrDefault(gc => gc.id == id);
            GolfCourseBO selCourse = Mapper.Map<GolfCourseBO>(golfCour);
            return selCourse;
        }
        public CourseTermBO SelectTermById(int id)
        {
            CourseTerm cTerm = _baseEntities.CourseTerm.FirstOrDefault(t => t.id == id);
            CourseTermBO selCTerm = Mapper.Map<CourseTermBO>(cTerm);
            return selCTerm;
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
                _baseEntities.Rent.Add(rentInfo);
                _baseEntities.SaveChanges();
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
                _logger.LogError("Error: Customer => SaveRent " + ex);
            }
            return true;
        }
        public bool SaveRentItems(int ctId, int custId, int[] equipIds)
        {
            try
            {
                Rent rent = _baseEntities.Rent.FirstOrDefault(r => r.custId == custId
                                                                && r.courTrmId == ctId
                                                                && r.billDate == m_rentConf);
                List<EquipmentBO> equip = GetSelEquipmentById(equipIds);
                if (rent != null)
                {
                    RentItems re = new RentItems();
                    foreach (var eq in equip)
                    {
                        re.equipId = eq.Id;
                        re.price = eq.Price;
                    }
                    _baseEntities.RentItems.Add(re);
                    _baseEntities.SaveChanges();
                }
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
                _logger.LogError("Error: Customer => SaveRentItems " + ex);
            }
            return true;
        }

    }
}