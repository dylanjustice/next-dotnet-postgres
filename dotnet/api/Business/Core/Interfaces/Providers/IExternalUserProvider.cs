using System.Collections.Generic;
using System.Threading.Tasks;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;

namespace DylanJustice.Demo.Api.Business.Core.Interfaces.Providers
{
    public interface IExternalUserProvider
    {
        Task<IEnumerable<User>> List();
    }
}