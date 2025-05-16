using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHealth.DTO.Administration.Doctor;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using System.Linq;
using UniHealth.Data;
using UniHealth.response.Models_Response;

namespace UniHealth.Controllers.Administration
{
    [Authorize(Roles = "Admin,Docotr")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public DoctorsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // Get all Doctors 
        [HttpGet("Get-All-Docotrs")]
        public async Task<APIResponse> GetAllDoctors()
        {
            try
            {
                var doctors = await _db.Doctors
                    .Include(d => d.User)
                    .Include(d => d.University)
                    .Select(d => _mapper.Map<DoctorResponseDto>(d))
                    .ToListAsync();

                return APIResponse.Success(doctors);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب بيانات الأطباء: {ex.Message}");
            }
        }

        // Get Doctor by ID
        [HttpGet("Get-Doctor-By-Id , {id}")]
        public async Task<APIResponse> GetDoctorById(int id)
        {
            try
            {
                var doctor = await _db.Doctors
                    .Include(d => d.User)
                    .Include(d => d.University)
                    .Where(d => d.Id == id)
                    .Select(d => _mapper.Map<DoctorResponseDto>(d))
                    .FirstOrDefaultAsync();

                if (doctor == null)
                {
                    return APIResponse.Fail(new List<string> { "الطبيب غير موجود" });
                }

                return APIResponse.Success(doctor);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب بيانات الطبيب: {ex.Message}");
            }
        }

        // Create new Doctor 
        [HttpPost("Create-New-Doctor")]
        public async Task<APIResponse> CreateDoctor([FromBody] CreateDoctorDTO createDto)
        {
            try
            {
                var user = await _db.Users.FindAsync(createDto.UserId.ToString());
                if (user == null || user.RoleId != 2)
                {
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود أو ليس طبيباً" });
                }

                var doctorExists = await _db.Doctors.AnyAsync(d => d.UserId == createDto.UserId.ToString());
                if (doctorExists)
                    return APIResponse.Fail(new List<string> { "الطبيب مسجل مسبقاً" });

                var universityExists = await _db.Universities.AnyAsync(u => u.Id == createDto.UniversityId);
                if (!universityExists)
                {
                    return APIResponse.Fail(new List<string> { "الجامعة غير موجودة" });
                }

                var doctor = _mapper.Map<Doctor>(createDto);
                _db.Doctors.Add(doctor);
                await _db.SaveChangesAsync();

                return APIResponse.Success(_mapper.Map<DoctorResponseDto>(doctor), "تم تسجيل الطبيب بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تسجيل الطبيب: {ex.Message}");
            }
        }

        // Update Doctor university By Id
        [HttpPut("Update-Doctor-University , {id}")]
        public async Task<APIResponse> UpdateDoctor(int id, [FromBody] UpdateDoctorDTO updateDto)
        {
            try
            {
                var doctor = await _db.Doctors.FindAsync(id);
                if (doctor == null)
                    return APIResponse.Fail(new List<string> { "الطبيب غير موجود" });

                var universityExists = await _db.Universities.AnyAsync(u => u.Id == updateDto.UniversityId);
                if (!universityExists)
                {
                    return APIResponse.Fail(new List<string> { "الجامعه غير موجوده" });
                }

                _mapper.Map(updateDto, doctor);
                _db.Doctors.Update(doctor);
                await _db.SaveChangesAsync();

                return APIResponse.Success(_mapper.Map<DoctorResponseDto>(doctor), "تم تحديث بيانات الطبيب بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تحديث بيانات الطبيب: {ex.Message}");
            }
        }

        // Remnove Doctor 
        [HttpDelete("Remove-Doctor-By-Id, {id}")]
        public async Task<APIResponse> DeleteDoctor(int id)
        {
            try
            {
                var doctor = await _db.Doctors.FindAsync(id);
                if (doctor == null)
                    return APIResponse.Fail(new List<string> { "الطبيب غير موجود" });

                var hasReports = await _db.Reports.AnyAsync(r => r.DoctorId == id);
                if (hasReports)
                {
                    return APIResponse.Fail(new List<string> { "لا يمكن حذف الطبيب لديه تقارير مرتبطة" });
                }

                _db.Doctors.Remove(doctor);
                await _db.SaveChangesAsync();

                return APIResponse.Success(null, "تم حذف الطبيب بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء حذف الطبيب: {ex.Message}");
            }
        }
    }
}