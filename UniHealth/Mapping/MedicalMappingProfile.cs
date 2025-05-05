using AutoMapper;
using UniHealth.DTO.Medical.MedicalRecord;
using UniHealth.DTO.Medical.Report;
using UniHealth.Models;
using UniHealth.response.Models_Response;

namespace UniHealth.Mapping
{
    public class MedicalMappingProfile : Profile
    {
        public MedicalMappingProfile()
        {
            // MedicalRecord
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();
            CreateMap<UpdateMedicalRecordDto, MedicalRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Report
            CreateMap<CreateReportDto, Report>();
            CreateMap<UpdateReportDto, Report>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Report -> ReportResponseDto
            CreateMap<Report, ReportResponseDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.User.FName} {src.User.LName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.User.FName} {src.Doctor.User.LName}"));
       
        }
    }
}
