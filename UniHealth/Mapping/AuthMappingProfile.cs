using AutoMapper;
using UniHealth.DTO.Auth;
using UniHealth.Models;
using UniHealth.response.Models_Response;

namespace UniHealth.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            // CreateUserDTO -> User
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.FName, opt => opt.MapFrom(src => src.FName))
                .ForMember(dest => dest.LName, opt => opt.MapFrom(src => src.LName))
                .ForMember(dest => dest.imageUrl, opt => opt.Ignore());

            // UpdateUserDto -> User
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.FName, opt => opt.MapFrom(src => src.FName))
                .ForMember(dest => dest.LName, opt => opt.MapFrom(src => src.LName))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // User -> UserResponseDto 
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FName} {src.LName}"))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
