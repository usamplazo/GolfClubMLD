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
        private GolfClubMldDBEntities _adminEntities = new GolfClubMldDBEntities();
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
                Users managerdb = new Users()
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
                Users custForEdit = _adminEntities.Users.FirstOrDefault(c => c.id == manager.Id);

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
            Equipment eq = _adminEntities.Equipment.FirstOrDefault(e => e.id == equip.Id);
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

        public bool EditCourse(GolfCourseBO course)
        {
            try
            {
                GolfCourse editedCourse = _adminEntities.GolfCourse.FirstOrDefault(c => c.id == course.Id);

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
            GolfCourse gc = _adminEntities.GolfCourse.FirstOrDefault(g => g.id == courseId);
            if (gc is null)
                return false;
            try
            {
                _adminEntities.GolfCourse.Remove(gc);
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