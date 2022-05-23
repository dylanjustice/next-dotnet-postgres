using AndcultureCode.CSharp.Conductors;
using System.Linq.Expressions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Mockaroo.Business.Core.Models.Users;
using AndcultureCode.CSharp.Core.Interfaces.Data;
using Mockaroo.Business.Core.Interfaces;
using Microsoft.Extensions.Logging;
using AndcultureCode.CSharp.Core;
using AndcultureCode.CSharp.Core.Extensions;

namespace Mockaroo.Business.Conductors.Domain
{
    public class UserReadConductor : IUserReadConductor
    {
        #region Private Members

        readonly ILogger<UserReadConductor> _logger;
        readonly IUsersProvider _usersProvider;

        #endregion Private Members
        public UserReadConductor(IUsersProvider usersProvider, ILogger<UserReadConductor> logger)
        {
            _usersProvider = usersProvider;
            _logger = logger;
        }

        #region IUserReadConductor Implementation

        public IResult<IEnumerable<User>> FindAll() => Do<IEnumerable<User>>.Try((r) =>
        {
            var usersResult = _usersProvider.ListUsers().Result;
            if (usersResult.HasErrors)
            {
                r.AddErrors(usersResult.Errors);
            }

            return usersResult.ResultObject;
        }).Result;

        #endregion IUserReadConductor Implementation
    }
}