using Graduation.Core.Features.Students.Commands.Models;
using Graduation.Core.Features.Students.Quaries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : AppControllerBase
    {
       

        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] GetStudentList getStudentList  )
        {
            var result = await Mediator.Send(getStudentList);
            return NewResult(result);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute]int id)
        {
            var reslut = await Mediator.Send(new GetStudentByIdQuery() { Id = id });
            return NewResult(reslut);
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent ([FromQuery] AddStudentCommand addStudentModel)
        {
            var reslut = await Mediator.Send(addStudentModel);
            return NewResult(reslut);
        }
    }
}
