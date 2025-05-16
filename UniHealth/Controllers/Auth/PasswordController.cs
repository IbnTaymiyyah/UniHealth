using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniHealth.DTO.Auth.Password;
using UniHealth.Models;
using UniHealth.response.API_Response;
using UniHealth.Services;
using UniHealth.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace UniHealth.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("GeneralPolicy")]
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _db;

        public PasswordController(
            UserManager<User> userManager,
            IEmailService emailService,
            AppDbContext context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _db = context;
        }

        // Change Password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<APIResponse> ChangePassword([FromBody] ChangePasswordDto model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });

                
                var isCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!isCorrect)
                    return APIResponse.Fail(new List<string> { "كلمة المرور الحالية غير صحيحة" });

                
                var result = await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);

                if (!result.Succeeded)
                    return APIResponse.Fail(result.Errors.Select(e => e.Description).ToList());

                return APIResponse.Success("تم تغيير كلمة المرور بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($"حدث خطأ أثناء تغيير كلمة المرور: {ex.Message}");
            }
        }

        // Foreget Password (Send Code)
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<APIResponse> ForgotPassword([FromBody] ForgetPasswordRequestDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return APIResponse.Fail(new List<string> { "البريد الإلكتروني غير مسجل" });

                
                var code = new Random().Next(100000, 999999).ToString();

                
                var resetCode = new PasswordReset
                {
                    UserId = user.Id,
                    Code = code,
                    ExpiryDate = DateTime.UtcNow.AddMinutes(15)
                };

                _db.PasswordReset.Add(resetCode);
                await _db.SaveChangesAsync();

                
                await _emailService.SendPasswordResetCodeAsync(user, code);

                return APIResponse.Success("تم إرسال كود التحقق إلى بريدك الإلكتروني");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($"حدث خطأ أثناء طلب إعادة تعيين كلمة المرور: {ex.Message}");
            }
        }

        // Foreget Password (Check Code & Change Password)
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<APIResponse> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return APIResponse.Fail(new List<string> { "البريد الإلكتروني غير مسجل" });

                
                var validCode = await _db.PasswordReset
                    .FirstOrDefaultAsync(options =>
                        options.UserId == user.Id &&
                        options.Code == model.VerificationCode &&
                        options.ExpiryDate > DateTime.UtcNow);

                if (validCode == null)
                    return APIResponse.Fail(new List<string> { "كود التحقق غير صحيح أو منتهي الصلاحية" });

                
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                
                var result = await _userManager.ResetPasswordAsync(
                    user,
                    resetToken,
                    model.NewPassword);

                if (!result.Succeeded)
                    return APIResponse.Fail(result.Errors.Select(e => e.Description).ToList());

                
                _db.PasswordReset.Remove(validCode);
                await _db.SaveChangesAsync();

                return APIResponse.Success("تم إعادة تعيين كلمة المرور بنجاح");
            }
            catch (Exception ex)
            {
                return APIResponse.Error($"حدث خطأ أثناء إعادة تعيين كلمة المرور: {ex.Message}");
            }
        }
    }
}