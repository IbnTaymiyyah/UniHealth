using AutoMapper;
using UniHealth.DTO.Academic.StudentDetail;
using UniHealth.DTO.Academic.University;
using UniHealth.Models;
using UniHealth.response.Models_Response;

namespace UniHealth.Mapping
{
    public class AcademicMappingProfile : Profile
    {
        public AcademicMappingProfile()
        {
            // StudentDetail
            CreateMap<CreateStudentDetailDto, StudentDetail>();
            CreateMap<UpdateStudentDetailDto, StudentDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // University
            CreateMap<CreateUniversityDto, University>();
            CreateMap<UpdateUniversityDto, University>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // StudentDetail -> StudentDetailResponseDto
            CreateMap<StudentDetail, StudentDetailResponseDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.User.FName} {src.User.LName}"))
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.Univeristy.Name))
                .ForMember(dest => dest.CollegeName, opt => opt.MapFrom(src => src.College.Name));
        }
        }
}
