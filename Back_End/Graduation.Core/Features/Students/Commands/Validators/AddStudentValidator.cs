using Graduation.Core.Features.Students.Commands.Models;
using Graduation.Service.Abstract;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Students.Commands.Validators
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        private readonly IStudentService studentService;

        public AddStudentValidator(IStudentService studentService)
        {
            this.studentService = studentService;
            ApplyRules();
            ApplyCustomRules();
          
        }
        private void ApplyRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
            RuleFor(x => x.Age)
                .InclusiveBetween(1, 120).WithMessage("Age must be between 1 and 120.");
        }
        private  void ApplyCustomRules()
        {
            RuleFor(x => x.Name).MustAsync(async (Key, cancellationToken) =>
            
                !await studentService.IsNameExist(Key)
            );

        }
    }
    }
