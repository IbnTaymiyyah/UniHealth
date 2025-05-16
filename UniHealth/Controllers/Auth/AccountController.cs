using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniHealth.DTO.Auth;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using UniHealth.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniHealth.Data;
using UniHealth.response.Models_Response;

namespace UniHealth.Controllers.Auth
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;

        public AccountController(
            UserManager<User> userManager,
            IMapper mapper,
            AppDbContext context,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _db = context;
            _tokenService = tokenService;
        }

        // Change User Info
        [HttpPut("update-User-Info")]
        public async Task<APIResponse> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return APIResponse.Fail(new List<string> { "غير مصرح به" });

                var user = await _userManager.FindByIdAsync(userId);


                if (user == null)
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });

                
                user.FName = updateUserDto.FirstName;
                user.LName = updateUserDto.LastName;
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.ProfileImageUrl = updateUserDto.ProfileImageUrl ?? user.ProfileImageUrl;
                user.imageUrl = updateUserDto.ImageUrl ?? user.imageUrl;

                var result = await _userManager.UpdateAsync(user);



                if (!result.Succeeded)
                    return APIResponse.Fail(result.Errors.Select(e => e.Description).ToList());



                var updatedUser = await _db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var response = _mapper.Map<UserResponseDto>(updatedUser);
                return APIResponse.Success(response);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تحديث البيانات: {ex.Message}");
            }
        }

        // Change UserName
        [HttpPut("change-username")]
        public async Task<APIResponse> ChangeUsername([FromBody] ChangeUsernameDto changeUsernameDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return APIResponse.Fail(new List<string> { "غير مصرح به" });

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });

                
                if (!await _userManager.CheckPasswordAsync(user, changeUsernameDto.CurrentPassword))
                    return APIResponse.Fail(new List<string> { "كلمة المرور الحالية غير صحيحة" });

                
                var userExists = await _userManager.FindByNameAsync(changeUsernameDto.NewUsername);
                if (userExists != null)
                    return APIResponse.Fail(new List<string> { "اسم المستخدم الجديد موجود مسبقاً" });

               
                user.UserName = changeUsernameDto.NewUsername;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return APIResponse.Fail(result.Errors.Select(e => e.Description).ToList());

               
                var updatedUser = await _db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var newToken = _tokenService.GenerateToken(updatedUser);
                var userResponse = _mapper.Map<UserResponseDto>(updatedUser);

                var responseData = new
                {
                    Token = newToken,
                    User = userResponse
                };

                return APIResponse.Success(responseData);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تغيير اسم المستخدم: {ex.Message}");
            }
        }

        // Show User Info
        [HttpGet("user-info")]
        public async Task<APIResponse> GetUserInfo()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return APIResponse.Fail(new List<string> { "غير مصرح به" });

                var user = await _db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });

                var response = _mapper.Map<UserResponseDto>(user);
                return APIResponse.Success(response);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب بيانات المستخدم: {ex.Message}");
            }
        }
    }
}