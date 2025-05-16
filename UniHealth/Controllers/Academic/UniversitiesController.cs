using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHealth.DTO.Academic.University;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using System.Linq;
using UniHealth.Data;

namespace UniHealth.Controllers.Academic
{
   
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class UniversitiesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public UniversitiesController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        // Get list of all universities
        [HttpGet("all-Universities")]
        public async Task<APIResponse> GetAllUniversities()
        {
            try
            {
                var universities = await _db.Universities
                    .OrderBy(u => u.Name)
                    .ToListAsync();

                return APIResponse.Success(universities);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب الجامعات: {ex.Message}");
            }
        }


        // Get university details by ID
        [HttpGet("Universitie-By-Id , {id}")]
        public async Task<APIResponse> GetUniversityById(int id)
        {
            try
            {
                var university = await _db.Universities.FindAsync(id);

                if (university == null)
                {
                    return APIResponse.Fail(new List<string> { "الجامعة غير موجودة" });
                }

                return APIResponse.Success(university);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($"حدث خطأ أثناء جلب بيانات الجامعة: {ex.Message}");
            }
        }


        // Create new university 
        [HttpPost("create-New-Universitie")]
        public async Task<APIResponse> CreateUniversity([FromBody] CreateUniversityDto createDto)
        {
            try
            {
                var universityExists = await _db.Universities
                    .AnyAsync(u => u.Name == createDto.Name);

                if (universityExists)
                {
                    return APIResponse.Fail(new List<string> { "اسم الجامعة موجود مسبقاً" });
                }

                var university = _mapper.Map<University>(createDto);

                _db.Universities.Add(university);
                await _db.SaveChangesAsync();

                return APIResponse.Success(university, "تم إنشاء الجامعة بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء إنشاء الجامعة: {ex.Message}");
            }
        }


        // Update university information 
        [HttpPut("update-Universitie-Info, {id}")]
        public async Task<APIResponse> UpdateUniversity(int id, [FromBody] UpdateUniversityDto updateDto)
        {
            try
            {
                var university = await _db.Universities.FindAsync(id);

                if (university == null)
                    return APIResponse.Fail(new List<string> { "الجامعة غير موجودة" });

                var nameExists = await _db.Universities.AnyAsync(u => u.Name == updateDto.Name && u.Id != id);

                if (nameExists)
                {
                    return APIResponse.Fail(new List<string> { "اسم الجامعة موجود مسبقاً" });
                }

                _mapper.Map(updateDto, university);
                _db.Universities.Update(university);
                await _db.SaveChangesAsync();

                return APIResponse.Success(university, "تم تحديث الجامعة بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تحديث الجامعة: {ex.Message}");
            }
        }

       
        //Remove a University By Id
        
        [HttpDelete("Remove-Universitie-By-Id , {id}")]
        public async Task<APIResponse> DeleteUniversity(int id)
        {
            try
            {
                var university = await _db.Universities.FindAsync(id);

                if (university == null)
                    return APIResponse.Fail(new List<string> { "الجامعة غير موجودة" });

             
                var hasStudents = await _db.StudentDetails.AnyAsync(s => s.UniveristyId == id);
                var hasDoctors = await _db.Doctors.AnyAsync(d => d.UniversityId == id);

                if (hasStudents || hasDoctors)
                {
                    return APIResponse.Fail(new List<string> { "لا يمكن حذف الجامعة ...الجامعه مرتبطة بطلاب أو أطباء" });
                }

                _db.Universities.Remove(university);
                await _db.SaveChangesAsync();

                return APIResponse.Success(null, "تم حذف الجامعة بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء حذف الجامعة: {ex.Message}");
            }
        }
    }
}