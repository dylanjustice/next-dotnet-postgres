using AutoMapper;
using Mockaroo.Business.Core.Models.Users;
using Mockaroo.Infrastructure.Data.Mockaroo.Models;

namespace Mockaroo.Infrastructure.Data.Mockaroo.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MockarooUserDto, User>();
        }

    }
}