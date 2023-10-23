using AutoMapper;
using BusinessObjects.Models;
using FUCarRentingSystem.DTO;

namespace FUCarRentingSystem.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<CarInformationDto, CarInformation>().ReverseMap();
        }
    }
}
