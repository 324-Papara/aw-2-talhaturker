using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data.Repositories;
using SchoolAPI.Models;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Student> _validator;

        public StudentsController(IUnitOfWork unitOfWork, IValidator<Student> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _unitOfWork.Students.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            var validationResult = await _validator.ValidateAsync(student);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student student)
        {
            if (id != student.Id) return BadRequest();

            var validationResult = await _validator.ValidateAsync(student);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return NotFound();

            _unitOfWork.Students.Remove(student);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var students = _unitOfWork.Students.Find(s => s.Name.Contains(name));
            return Ok(await students.ToListAsync());
        }

        [HttpGet("include-courses")]
        public async Task<IActionResult> GetStudentsWithCourses()
        {
            var students = _unitOfWork.Students.Include(s => s.Courses);
            return Ok(await students.ToListAsync());
        }
    }

}
