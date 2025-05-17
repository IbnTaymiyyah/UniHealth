using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHealth.DTO.Medical.Report;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using System.Linq;
using UniHealth.Data;
using UniHealth.response.Models_Response;

namespace UniHealth.Controllers.Medical
{
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ReportsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // Get All Reports with student and doctor names
        [HttpGet("Get-All-Reports")]
        public async Task<APIResponse> GetAllReports()
        {
            try
            {
                var reports = await _db.Reports
                    .Include(r => r.User)
                    .Include(r => r.Doctor)
                    .ThenInclude(d => d.User)
                    .OrderByDescending(r => r.Id)
                    .Select(r => _mapper.Map<ReportResponseDto>(r))
                    .ToListAsync();

                return APIResponse.Success(reports);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب التقارير: {ex.Message}");
            }
        }

        // Get Report by ID
        [HttpGet("Get-Report-By-Id/{id}")]
        public async Task<APIResponse> GetReportById(int id)
        {
            try
            {
                var report = await _db.Reports
                    .Include(r => r.User)
                    .Include(r => r.Doctor)
                    .ThenInclude(d => d.User)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (report == null)
                {
                    return APIResponse.Fail(new List<string> { "التقرير غير موجود" });
                }

                var result = new
                {
                    report.Id,
                    StudentName = $"{report.User.FName} {report.User.LName}",
                    DoctorName = $"{report.Doctor.User.FName} {report.Doctor.User.LName}",
                    report.Title,
                    report.Description,
                    report.Notes
                };

                return APIResponse.Success(result);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب التقرير: {ex.Message}");
            }
        }

        // Create new Report 
        [Authorize(Roles = "Doctor")]
        [HttpPost("Create-New-Report")]
        public async Task<APIResponse> CreateReport([FromBody] CreateReportDto createDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                
                var doctorUser = await _db.Users
                    .Include(u => u.Doctor)
                    .FirstOrDefaultAsync(u => u.Id == createDto.UserId.ToString());

                if (doctorUser == null || doctorUser.Doctor == null)
                {
                    return APIResponse.Fail(new List<string> { "الطبيب غير موجود" });
                }

                var report = _mapper.Map<Report>(createDto);
                report.DoctorId = doctorUser.Doctor.Id;

                await _db.Reports.AddAsync(report);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(report, "تم إنشاء التقرير بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء إنشاء التقرير: {ex.Message}");
            }
        }

        // Update Report 
        [Authorize(Roles = "Doctor")]
        [HttpPut("Update-Report/{id}")]
        public async Task<APIResponse> UpdateReport(int id, [FromBody] UpdateReportDto updateDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var report = await _db.Reports.FindAsync(id);
                if (report == null)
                {
                    return APIResponse.Fail(new List<string> { "التقرير غير موجود" });
                }

                _mapper.Map(updateDto, report);
                _db.Reports.Update(report);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(report, "تم تحديث التقرير بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء تحديث التقرير: {ex.Message}");
            }
        }

        // Remove Report 
        [HttpDelete("Remove-Report/{id}")]
        public async Task<APIResponse> DeleteReport(int id)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var report = await _db.Reports.FindAsync(id);
                if (report == null)
                {
                    return APIResponse.Fail(new List<string> { "التقرير غير موجود" });
                }

                _db.Reports.Remove(report);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(null, "تم حذف التقرير بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء حذف التقرير: {ex.Message}");
            }
        }
    }
}