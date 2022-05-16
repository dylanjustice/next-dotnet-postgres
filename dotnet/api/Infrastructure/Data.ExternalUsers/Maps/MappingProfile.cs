using AutoMapper;
using DylanJustice.Demo.Business.Core.Models.Dto;
using DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Models;

namespace DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ExternalEmployeeDto, Employee>();
        }

    }
}