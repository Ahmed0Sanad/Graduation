using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Service.Abstract
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> getAll();
        Task<bool> IsNameExist(string name);
    }
}
