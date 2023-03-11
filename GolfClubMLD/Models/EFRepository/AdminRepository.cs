using AutoMapper.QueryableExtensions;
using GolfClubMLD.Controllers;
using GolfClubMLD.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace GolfClubMLD.Models.EFRepository
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        private GolfClbMldDBEntities _adminEntities = new GolfClbMldDBEntities();
        private readonly Logger<AdminController> _logger;
        public AdminRepository()
        {

        }
        public AdminRepository(Logger<AdminController> logger)
        {
            _logger = logger;
        }
        public List<UsersBO> GetAllUsers()
        {
            List<UsersBO> allUsers = _adminEntities.Users
                                                        .Where(u => u.isActv && (u.roleId == 1 || u.roleId == 2))
                                                        .ProjectTo<UsersBO>()
                                                        .ToList();
            // 1 - Customer
            // 2 - Manager

            return allUsers;
        }

        public bool RegisterManager(UsersBO manager)
        {
            if (manager is null)
                return false;

            try
            {
                User managerdb = new User()
                {
                    email = manager.Email,
                    username = manager.Username,
                    pass = manager.Pass,
                    fname = manager.Fname,
                    lname = manager.Lname,
                    phone = manager.Phone,
                    profPic = manager.ProfPic,
                    isActv = true,
                    roleId = manager.RoleId
                };
                _adminEntities.Users.Add(managerdb);
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Authentification => RegisterCustomer " + ex);
            }
            return true;
        }

        public bool EditManagerData(UsersBO manager)
        {
            try
            {
                User custForEdit = _adminEntities.Users.FirstOrDefault(c => c.id == manager.Id);

                if (custForEdit is null)
                    return false;

                custForEdit.username = manager.Username;
                custForEdit.pass = HashPassword(manager.Pass);
                custForEdit.fname = manager.Fname;
                custForEdit.lname = manager.Lname;
                custForEdit.phone = manager.Phone;
                custForEdit.profPic = manager.ProfPic;


                _adminEntities.Entry(custForEdit).State = EntityState.Modified;
                _adminEntities.SaveChanges();

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
                _logger.LogError("Error: Admin => EditManagerData " + ex);
            }
            return true;
        }

        public bool UpdateEquipment(EquipmentBO equip)
        {
            Equipment eq = _adminEntities.Equipments.FirstOrDefault(e => e.id == equip.Id);
            if (eq is null)
            {
                return false;
            }
            AutoMapper.Mapper.Map(equip, eq);
            try
            {
                _adminEntities.Entry(eq).State = EntityState.Modified;
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Admin => UpdateEquipment " + ex);
            }
            return true;

        }

        public bool RemoveEquipment(EquipmentBO equip)
        {
            try
            {
                Equipment eq = _adminEntities.Equipments.FirstOrDefault(e => e.id == equip.Id);

                if (eq is null)
                    return false;

                _adminEntities.Equipments.Remove(eq);
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Admin => EditGolfCourse " + ex);
            }
            return true;
        }

        public bool CreateEquipment(EquipmentBO equip)
        {
            try
            {

                Equipment newEq = new Equipment();
                AutoMapper.Mapper.Map(equip, newEq);

                _adminEntities.Equipments.Add(newEq);
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Admin => CreateEquipment " + ex);
            }
            return true;
        }

        public bool EditCourse(GolfCourseBO course)
        {
            try
            {
                GolfCourse editedCourse = _adminEntities.GolfCourses.FirstOrDefault(c => c.id == course.Id);

                if (editedCourse is null)
                    return false;

                AutoMapper.Mapper.Map(course, editedCourse);

                _adminEntities.Entry(editedCourse).State = EntityState.Modified;
                _adminEntities.SaveChanges();

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
                _logger.LogError("Error: Admin => EditGolfCourse " + ex);
            }
            return true;
        }

        public bool DelCourse(int courseId)
        {
            GolfCourse gc = _adminEntities.GolfCourses.FirstOrDefault(g => g.id == courseId);
            if (gc is null)
                return false;
            try
            {
                _adminEntities.GolfCourses.Remove(gc);
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Admin => EditGolfCourse " + ex);
            }
            return true;

        }

        public bool CreateCourse(GolfCourseBO course)
        {
            try
            {
                GolfCourse newGolfCor = new GolfCourse();
                AutoMapper.Mapper.Map(course, newGolfCor);

                _adminEntities.GolfCourses.Add(newGolfCor);
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Admin => EditGolfCourse " + ex);
            }
            return true;
        }
        public IEnumerable<TermBO> GetAllTerms()
        {
            IEnumerable<Term> terms = _adminEntities.Terms.ToList();
            List<TermBO> termsBO = new List<TermBO>();

            AutoMapper.Mapper.Map(terms, termsBO);

            return termsBO;
        }
        public bool CreateTerm(TermBO term)
        {
            try
            {
                string latestTerm = _adminEntities.Terms.Max(tr => tr.endTime);

                if (DateTime.Parse(term.StartTime) > DateTime.Parse(latestTerm))
                    return false;
                DateTime startWorkingHours = new DateTime(2100,12,31,07,00,00);
                DateTime endWorkingHours = new DateTime(2100,12,31,23,00,00);

                DateTime inputStartTime = DateTime.Parse(term.StartTime);
                DateTime inputEndTime = DateTime.Parse(term.EndTime);

                if (inputStartTime.TimeOfDay < startWorkingHours.TimeOfDay 
                    || inputStartTime.TimeOfDay > endWorkingHours.TimeOfDay)
                    return false;

                if (inputEndTime.TimeOfDay > endWorkingHours.TimeOfDay
                    || inputEndTime.TimeOfDay < inputStartTime.TimeOfDay)
                    return false;

                Term newTrm = new Term();
                AutoMapper.Mapper.Map(term, newTrm);

                _adminEntities.Terms.Add(newTrm);
                _adminEntities.SaveChanges();

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
                _logger.LogError("Error: Admin => EditGolfCourse " + ex);
            }
            return true;
        }

        public int GetTermId()
        {
            return _adminEntities.Terms.Max(t => t.id);
        }

        public bool CreateCourseTerm(int termId, int courseId, string day)
        {
            try
            {
                CourseTerm ct = new CourseTerm()
                {
                    termId = termId,
                    courseId = courseId,
                    dayOfW = day
                };

                _adminEntities.CourseTerms.Add(ct);
                _adminEntities.SaveChanges();
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
                _logger.LogError("Error: Admin => EditGolfCourse " + ex);
            }
            return true;
        }

    }
}