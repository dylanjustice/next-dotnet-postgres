using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using AndcultureCode.CSharp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mockaroo.Business.Core.Interfaces;
using Mockaroo.Business.Core.Models.Users;

namespace Mockaroo.Presentation.Web.Controllers
{
    [Route("/api/v1/users")]
    public class UsersController : AndcultureCode.CSharp.Web.Controllers.Controller
    {
        #region Private Members

        private readonly IUserReadConductor _userReadConductor;
        private readonly ILogger<UsersController> _logger;

        #endregion Private Members

        #region Constructors

        public UsersController(
            IUserReadConductor userReadConductor,
            ILogger<UsersController> logger
        ) : base(null)
        {
            _userReadConductor = userReadConductor;
            _logger = logger;
        }

        #endregion Constructors

        #region HTTP GET

        [HttpGet]
        public IActionResult List()
        {
            var userReadResult = _userReadConductor.FindAll();
            if (userReadResult.HasErrors)
            {
                return InternalError<IEnumerable<User>>(userReadResult.ResultObject, userReadResult.Errors, _logger);
            }

            return Ok<IEnumerable<User>>(userReadResult.ResultObject);
        }

        #endregion HTTP GET



    }
}