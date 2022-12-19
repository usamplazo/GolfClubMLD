using AutoMapper.QueryableExtensions;
using GolfClubMLD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GolfClubMLD.Models.EFRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private GolfClubMldDBEntities _custEntities = new GolfClubMldDBEntities();
        public async Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId)
        {
            Task<List<CourseTermBO>> terms = _custEntities.CourseTerm.Select(ct=>ct)
                                                .Where(ct=>ct.courseId == courseId)
                                                .Include(ct => ct.GolfCourse)
                                                .Include(ct=>ct.Term)
                                                .ProjectTo<CourseTermBO>()
                                                .ToListAsync();

            return await terms;                                  
        }
    }
}