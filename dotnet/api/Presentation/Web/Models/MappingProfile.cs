using AutoMapper;
using DylanJustice.Demo.Business.Core.Models.Entities.Roles;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;
using DylanJustice.Demo.Business.Core.Models.Jobs;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Jobs;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Roles;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Users;
using DylanJustice.Demo.Business.Core.Models.Storage;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Employees;
using DylanJustice.Demo.Business.Core.Models.Dto;

namespace DylanJustice.Demo.Presentation.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            // Jobs
            CreateMap<Job, JobDto>();

            // RemoteAccessDetails
            CreateMap<RemoteAccessDetails, RemoteAccessDetailsDto>();

            // Roles
            CreateMap<Role, RoleDto>();

            // Users
            CreateMap<User, UserDto>();

            // UserLogins
            CreateMap<UserLogin, UserLoginDto>();

            // UserRoles
            CreateMap<UserRole, UserRoleDto>();
        }
    }
}
