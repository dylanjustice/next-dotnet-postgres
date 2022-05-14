using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Mockaroo.Business.Core.Models.Users;

namespace Mockaroo.Business.Core.Interfaces
{
    public interface IUserReadConductor
    {
        IResult<IEnumerable<User>> FindAll();
    }
}