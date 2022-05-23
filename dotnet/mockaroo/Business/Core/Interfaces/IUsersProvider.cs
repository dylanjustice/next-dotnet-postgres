using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Interfaces.Providers;
using Mockaroo.Business.Core.Models.Users;

namespace Mockaroo.Business.Core.Interfaces
{
    public interface IUsersProvider : IProvider
    {
        Task<IResult<List<User>>> ListUsers();
    }
}