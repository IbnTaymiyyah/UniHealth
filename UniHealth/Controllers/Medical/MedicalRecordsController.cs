using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHealth.DTO.Medical.MedicalRecord;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using System.Linq;
using UniHealth.Data;

namespace UniHealth.Controllers.Medical
{
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public MedicalRecordsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // Get all Medical records
        [HttpGet("Get-All-Medical")]
        public async Task<APIResponse> GetAllMedicalRecords()
        {
            try
            {
                var records = await _db.MedicalRecords
                    .Include(m => m.User)
                    .OrderByDescending(m => m.createdAt)
                    .ToListAsync();

                return APIResponse.Success(records);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب السجلات الطبية: {ex.Message}");
            }
        }

        // Get Medical record by ID
        [HttpGet("Get-Medical-Record-By-Id/{id}")]
        public async Task<APIResponse> GetMedicalRecordById(int id)
        {
            try
            {
                var record = await _db.MedicalRecords
                    .Include(m => m.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (record == null)
                {
                    return APIResponse.Fail(new List<string> { "السجل الطبي غير موجود" });
                }

                return APIResponse.Success(record);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب السجل الطبي: {ex.Message}");
            }
        }

        // Create new Medical 
        [HttpPost("Create-New-Medical")]
        public async Task<APIResponse> CreateMedicalRecord([FromBody] CreateMedicalRecordDto createDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var user = await _db.Users.FindAsync(createDto.UserId.ToString());
                if (user == null)
                {
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });
                }

                var record = _mapper.Map<MedicalRecord>(createDto);
                record.createdAt = DateTime.Now;

                await _db.MedicalRecords.AddAsync(record);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(record, "تم إنشاء السجل الطبي بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء إنشاء السجل الطبي: {ex.Message}");
            }
        }

        // Update Medical record
        [HttpPut("Update-Medical-Record/{id}")]
        public async Task<APIResponse> UpdateMedicalRecord(int id, [FromBody] UpdateMedicalRecordDto updateDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var record = await _db.MedicalRecords.FindAsync(id);
                if (record == null)
                {
                    return APIResponse.Fail(new List<string> { "السجل الطبي غير موجود" });
                }

                _mapper.Map(updateDto, record);
                _db.MedicalRecords.Update(record);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(record, "تم تحديث السجل الطبي بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء تحديث السجل الطبي: {ex.Message}");
            }
        }

        // Delete medical record
        [HttpDelete("Remove-Medical-Record/{id}")]
        public async Task<APIResponse> DeleteMedicalRecord(int id)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var record = await _db.MedicalRecords.FindAsync(id);
                if (record == null)
                {
                    return APIResponse.Fail(new List<string> { "السجل الطبي غير موجود" });
                }

                _db.MedicalRecords.Remove(record);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return APIResponse.Success(null, "تم حذف السجل الطبي بنجاح");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse.Error($" خطأ أثناء حذف السجل الطبي: {ex.Message}");
            }
        }
    }
}