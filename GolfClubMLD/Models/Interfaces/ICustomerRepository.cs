using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfClubMLD.Models.Interfaces
{
    internal interface ICustomerRepository
    {
        Task<List<CourseTermBO>> GetTermsForSelCourse(int courseId);
    }
}
