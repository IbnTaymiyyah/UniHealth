using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHealth.DTO.Academic.StudentDetail;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using UniHealth.Data;

namespace UniHealth.Controllers.Academic
{
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public StudentsController(
            AppDbContext context,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _db = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Get All Students
        [HttpGet("Get-All-Students")]
        public async Task<APIResponse> GetAllStudents()
        {
            try
            {
                var students = await _db.StudentDetails
                    .Include(s => s.User)
                    .Include(s => s.College)
                    .Include(s => s.Univeristy)
                    .Select(s => new
                    {
                        s.Id,
                        StudentId = s.UserId,
                        StudentName = $"{s.User.FName} {s.User.LName}",
                        s.User.Email,
                        s.User.PhoneNumber,
                        CollegeName = s.College.Name,
                        UniversityName = s.Univeristy.Name
                    })
                    .ToListAsync();

                return APIResponse.Success(students);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب بيانات الطلاب: {ex.Message}");
            }
        }

        // Get Student By Id
        [HttpGet("Get-Student-By-Id/{id}")]
        public async Task<APIResponse> GetStudentById(int id)
        {
            try
            {
                var student = await _db.StudentDetails
                    .Include(s => s.User)
                    .Include(s => s.College)
                    .Include(s => s.Univeristy)
                    .Where(s => s.Id == id)
                    .Select(s => new
                    {
                        s.Id,
                        StudentId = s.UserId,
                        StudentName = $"{s.User.FName} {s.User.LName}",
                        s.User.Email,
                        s.User.PhoneNumber,
                        s.User.imageUrl,
                        CollegeName = s.College.Name,
                        UniversityName = s.Univeristy.Name,
                        CollegeId = s.CollegeId,
                        UniversityId = s.UniveristyId
                    })
                    .FirstOrDefaultAsync();

                if (student == null)
                    return APIResponse.Fail(new List<string> { "الطالب غير موجود" });

                return APIResponse.Success(student);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب بيانات الطالب: {ex.Message}");
            }
        }

        // Create New Student
        
        [HttpPost("Create-New-Student")]
        public async Task<APIResponse> CreateStudentDetail([FromBody] CreateStudentDetailDto createDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                
                var user = await _userManager.FindByIdAsync(createDto.UserId.ToString());
                if (user == null)
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });

                
                if (user.RoleId != 1)
                    return APIResponse.Fail(new List<string> { "المستخدم ليس طالب" });

                
                var studentExists = await _db.StudentDetails
                    .AnyAsync(s => s.UserId == createDto.UserId);

                if (studentExists)
                    return APIResponse.Fail(new List<string> { "بيانات الطالب موجودة مسبقاً" });

                
                var universityExists = await _db.Universities.AnyAsync(u => u.Id == createDto.UniversityId);

                var collegeExists = await _db.Colleges.AnyAsync(c => c.Id == createDto.CollegeId);

                if (!universityExists || !collegeExists)
                    return APIResponse.Fail(new List<string> { "الجامعة أو الكلية غير موجودة" });

                var studentDetail = _mapper.Map<StudentDetail>(createDto);
                studentDetail.UserId = user.Id;

                await _db.StudentDetails.AddAsync(studentDetail);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(studentDetail, "تم إنشاء بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء إنشاء بيانات الطالب: {ex.Message}");
            }
        }

        // Update Student Info

        [HttpPut("Update-Student-Info/{id}")]
        public async Task<APIResponse> UpdateStudentDetail(int id, [FromBody] UpdateStudentDetailDto updateDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var studentDetail = await _db.StudentDetails.FindAsync(id);
                if (studentDetail == null)
                    return APIResponse.Fail(new List<string> { "بيانات الطالب غير موجودة" });

                
                var universityExists = await _db.Universities
                    .AnyAsync(u => u.Id == updateDto.UniversityId);

                var collegeExists = await _db.Colleges
                    .AnyAsync(c => c.Id == updateDto.CollegeId);

                if (!universityExists || !collegeExists)
                    return APIResponse.Fail(new List<string> { "الجامعة أو الكلية غير موجودة" });

                _mapper.Map(updateDto, studentDetail);
                _db.StudentDetails.Update(studentDetail);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(studentDetail, "تم تحديث بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء تحديث بيانات الطالب: {ex.Message}");
            }
        }

        // Remove Student By Id

        [HttpDelete("Remove-Student-By-Id/{id}")]
        public async Task<APIResponse> DeleteStudentDetail(int id)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var studentDetail = await _db.StudentDetails.FindAsync(id);
                if (studentDetail == null)
                    return APIResponse.Fail(new List<string> { "بيانات الطالب غير موجودة" });

                _db.StudentDetails.Remove(studentDetail);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(null, "تم حذف بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء حذف بيانات الطالب: {ex.Message}");
            }
        }
    }
}