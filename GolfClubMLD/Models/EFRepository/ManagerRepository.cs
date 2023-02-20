using AutoMapper;
using GolfClubMLD.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace GolfClubMLD.Models.EFRepository
{
    public class ManagerRepository: BaseRepository, IManagerRepository
    {
        private GolfClubMldDBEntities _managEntities = new GolfClubMldDBEntities();
        private readonly ILogger<ManagerRepository> _logger;
        public ManagerRepository(ILogger<ManagerRepository> logger)
        {
            _logger = logger;
        }
        public ManagerRepository()
        {

        }
        public bool SaveEditedEquipment(EquipmentBO equipToEdit)
        {
            int? eqId = _managEntities.Equipment.Select(e=>e.id).FirstOrDefault(e => e == equipToEdit.Id);
            if (eqId == null)
                return false;
            try
            {
                Equipment equipToSave = Mapper.Map<Equipment>(equipToEdit);
                _managEntities.Entry(equipToSave).State = EntityState.Modified;
                _managEntities.SaveChanges();
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
                _logger.LogError("Error: Manager => EditEquipment " + ex);
            }
            return true;
        }
    }
}