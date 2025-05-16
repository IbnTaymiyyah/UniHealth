using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniHealth.Models;
using UniHealth.response.API_Response;
using AutoMapper;
using UniHealth.Data;
using Microsoft.AspNetCore.RateLimiting;
using UniHealth.DTO.Auth;
using UniHealth.response.Models_Response;
using UniHealth.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UniHealth.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("GeneralPolicy")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

       
        public AuthController(
            AppDbContext context,
            IMapper mapper,
            IConfiguration configuration,
            ITokenService tokenService,
            UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        // Register Student
        [AllowAnonymous]
        [HttpPost("register/student")]
        public async Task<APIResponse> RegisterStudent([FromBody] CreateUserDTO userDto)
        {
            try
            {
                if (userDto.RoleId != 1)
                    return APIResponse.Fail(new List<string> { "يمكن التسجيل كطالب فقط من هذه الواجهة" });

                var userExists = await _userManager.Users.AnyAsync(u => u.UserName == userDto.Username || u.Email == userDto.Email);
                if (userExists)
                    return APIResponse.Fail(new List<string> { "اسم المستخدم أو البريد الإلكتروني مسجل مسبقًا" });

                var user = new User
                {
                    UserName = userDto.Username,
                    Email = userDto.Email,
                    FName = userDto.FirstName,
                    LName = userDto.LastName,
                    RoleId = userDto.RoleId,
                    ProfileImageUrl = userDto.ProfileImageUrl
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                    return APIResponse.Fail(result.Errors.Select(e => e.Description).ToList());

                var studentDetail = new StudentDetail
                {
                    UserId = user.Id,
                    UniveristyId = 1,
                    CollegeId = 1
                };

                await _context.StudentDetails.AddAsync(studentDetail);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<UserResponseDto>(user);
                return APIResponse.Success(response);
            }
            catch (Exception ex)
            {
                string errorDetails = ex.InnerException != null ?
        $"{ex.Message} - {ex.InnerException.Message}" :
        ex.Message;

                return APIResponse.Error($"حدث خطأ أثناء تسجيل الطالب: {errorDetails}");
                // return APIResponse.Error($"حدث خطأ أثناء تسجيل الطالب: {ex.Message}");
            }
        }

        // Register Doctor
        [AllowAnonymous]
        [HttpPost("register/doctor")]
        public async Task<APIResponse> RegisterDoctor([FromBody] CreateUserDTO userDto)
        {
            try
            {
                if (userDto.RoleId != 2)
                    return APIResponse.Fail(new List<string> { "يمكن التسجيل كطبيب فقط من هذه الواجهة" });

                
                var universityId = await _context.DoctorIds
                    .FirstOrDefaultAsync(d => d.DoctorUniversityId == userDto.DoctorUniversityId &&
                                             !d.IsUsed);

                if (universityId == null)
                    return APIResponse.Fail(new List<string> { "Id  غير صحيح أو تم استخدامه " });

                var userExists = await _userManager.Users.AnyAsync(u => u.UserName == userDto.Username || u.Email == userDto.Email);
                if (userExists)
                    return APIResponse.Fail(new List<string> { "اسم المستخدم أو البريد الإلكتروني مسجل مسبقًا" });

                var user = new User
                {
                    UserName = userDto.Username,
                    Email = userDto.Email,
                    FName = userDto.FirstName,
                    LName = userDto.LastName,
                    RoleId = userDto.RoleId,
                    ProfileImageUrl = userDto.ProfileImageUrl
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                    return APIResponse.Fail(result.Errors.Select(e => e.Description).ToList());

                var doctor = new Doctor
                {
                    UserId = user.Id,
                    UniversityId = 1 
                };

                
                universityId.IsUsed = true;
                _context.DoctorIds.Update(universityId);

                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<UserResponseDto>(user);
                return APIResponse.Success(response);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء تسجيل الطبيب: {ex.Message}");
            }
        }

        // Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<APIResponse> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);

                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.CurrentPassword))
                    return APIResponse.Fail(new List<string> { "اسم المستخدم أو كلمة المرور غير صحيحة" });

                user = await _context.Users
                                .Include(u => u.Role) 
                                .FirstOrDefaultAsync(u => u.Id == user.Id);

                var token = _tokenService.GenerateToken(user);
                var userResponse = _mapper.Map<UserResponseDto>(user);
                string userType = user.RoleId == 3 ? "Doctor" : "Student";

                var responseData = new
                {
                    Token = token,
                    User = userResponse,
                    UserType = userType
                };

                return APIResponse.Success(responseData);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($"حدث خطأ أثناء تسجيل الدخول: {ex.Message}");
            }
        }

       
        // Get Profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<APIResponse> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return APIResponse.Fail(new List<string> { "لا تمتلك الصلاحيه" });
                }

                
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.FName,
                        u.LName,
                        u.PhoneNumber,
                        u.ProfileImageUrl,
                        u.imageUrl,
                        RoleId = u.RoleId,
                        RoleName = u.Role.Name
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return APIResponse.Fail(new List<string> { "المستخدم غير موجود" });
                }

                object profileData = null;

                if (user.RoleId == 1) 
                {
                    var studentData = await _context.StudentDetails
                        .Where(s => s.UserId == userId)
                        .Select(s => new
                        {
                            s.Id,
                            CollegeName = s.College.Name,
                            UniversityName = s.Univeristy.Name
                        })
                        .FirstOrDefaultAsync();

                    profileData = new { user, studentData };
                }
                else if (user.RoleId == 2) 
                {
                    var doctorData = await _context.Doctors
                        .Where(d => d.UserId == userId)
                        .Select(d => new
                        {
                            d.Id,
                            UniversityName = d.University.Name
                        })
                        .FirstOrDefaultAsync();

                    profileData = new { user, doctorData };
                }

                return APIResponse.Success(profileData ?? user);
            }
            catch (Exception ex)
            {
                return APIResponse.Error($" خطأ أثناء جلب الملف الشخصي: {ex.Message}");
            }
        }



    }
}