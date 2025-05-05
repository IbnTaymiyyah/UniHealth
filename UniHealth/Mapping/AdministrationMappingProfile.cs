using AutoMapper;
using UniHealth.DTO.Administration.College;
using UniHealth.DTO.Administration.Doctor;
using UniHealth.DTO.Administration.Role;
using UniHealth.Models;
using UniHealth.response.Models_Response;

namespace UniHealth.Mapping
{
    public class AdministrationMappingProfile : Profile
    {
        public AdministrationMappingProfile()
        {
            // College
            CreateMap<CreateCollegeDto, College>();
            CreateMap<UpdateCollegeDto, College>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Doctor
            CreateMap<CreateDoctorDTO, Doctor>();
            CreateMap<UpdateDoctorDTO, Doctor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Role
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Doctor -> DoctorResponseDto
            CreateMap<Doctor, DoctorResponseDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.User.FName} {src.User.LName}"))
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University.Name));

        }
    }
}
