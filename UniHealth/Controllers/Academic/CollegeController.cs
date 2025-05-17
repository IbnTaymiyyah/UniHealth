using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHealth.DTO.Administration.College;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using System.Security.Claims;
using System.Linq;
using UniHealth.Data;

namespace UniHealth.Controllers.Academic
{
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CollegeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get All Colleges
        [HttpGet("All-Colleges")]
        public async Task<APIResponse> GetAllColleges()
        {
            try
            {
                var colleges = await _context.Colleges
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return APIResponse.Success(colleges);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب الكليات: {ex.Message}");
            }
        }

        // Get College By Id
        [HttpGet("Get-College-By-Id/{id}")]
        public async Task<APIResponse> GetCollegeById(int id)
        {
            try
            {
                var college = await _context.Colleges.FindAsync(id);

                if (college == null)
                    return APIResponse.Fail(new List<string> { "الكلية غير موجودة" });

                return APIResponse.Success(college);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب الكلية: {ex.Message}");
            }
        }

        // Create New College
        [HttpPost("New-College")]
        public async Task<APIResponse> CreateCollege([FromBody] CreateCollegeDto createCollegeDto)
        {
            try
            {
                
                var collegeExists = await _context.Colleges.AnyAsync(c => c.Name == createCollegeDto.Name);

                if (collegeExists)
                    return APIResponse.Fail(new List<string> { "اسم الكلية موجود مسبقاً" });

                var college = _mapper.Map<College>(createCollegeDto);

                _context.Colleges.Add(college);
                await _context.SaveChangesAsync();

                return APIResponse.Success(college, "تم إنشاء الكلية بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء إنشاء الكلية: {ex.Message}");
            }
        }

        // Update College Info
        [HttpPut("Update-College-Info/{id}")]
        public async Task<APIResponse> UpdateCollege(int id, [FromBody] UpdateCollegeDto updateCollegeDto)
        {
            try
            {
                var college = await _context.Colleges.FindAsync(id);

                if (college == null)
                    return APIResponse.Fail(new List<string> { "الكلية غير موجودة" });

                
                var nameExists = await _context.Colleges.AnyAsync(c => c.Name == updateCollegeDto.Name && c.Id != id);

                if (nameExists)
                {
                    return APIResponse.Fail(new List<string> { "اسم الكلية موجود مسبقاً" });
                }

                _mapper.Map(updateCollegeDto, college);

                _context.Colleges.Update(college);
                await _context.SaveChangesAsync();

                return APIResponse.Success(college, "تم تحديث الكلية بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تحديث الكلية: {ex.Message}");
            }
        }

        // Remove College
        [HttpDelete("Remove-College/{id}")]
        public async Task<APIResponse> DeleteCollege(int id)
        {
            try
            {
                var college = await _context.Colleges.FindAsync(id);

                if (college == null)
                    return APIResponse.Fail(new List<string> { "الكلية غير موجودة" });

                
                var hasStudents = await _context.StudentDetails.AnyAsync(s => s.CollegeId == id);

                if (hasStudents)
                {
                    return APIResponse.Fail(new List<string> { "لا يمكن حذف الكلية ...الكليه مرتبطة بطلاب" });
                }
                _context.Colleges.Remove(college);
                await _context.SaveChangesAsync();

                return APIResponse.Success(null, "تم حذف الكلية بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($"حدث خطأ أثناء حذف الكلية: {ex.Message}");
            }
        }
    }
}