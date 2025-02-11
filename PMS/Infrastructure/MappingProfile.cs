using AutoMapper;
using PMS.Domain.Dtos;
using PMS.Domain.Entities;

namespace PMS.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PatientCreateDto, Patient>().ReverseMap();
            CreateMap<PatientUpdateDto, Patient>().ReverseMap();
            CreateMap<PatientResponseDto, Patient>().ReverseMap();
        }
    }
}
