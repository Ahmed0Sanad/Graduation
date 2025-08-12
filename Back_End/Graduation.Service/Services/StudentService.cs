using Graduation.Service.Abstract;
using Graduation.Infrustructure.Abstract;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        public async Task<IEnumerable<Student>> getAll()
        {
           return await  studentRepository.GetAllStudentsAsync();
        }

        public async Task<bool> IsNameExist(string name)
        {
            var stu = await studentRepository.GetAllStudentsAsync();
           if( stu.Select(x => x.Name.ToLower() == name.ToLower()).Any())
            {
                return true;
            }
            return false;

        }
    }
}
