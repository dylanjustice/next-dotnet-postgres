using System.Collections.Generic;
using AndcultureCode.CSharp.Web.Interfaces;
using AutoMapper;
using DylanJustice.Demo.Business.Core.Interfaces.Conductors.Domain.Employees;
using DylanJustice.Demo.Presentation.Web.Attributes;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace DylanJustice.Demo.Presentation.Web.Controllers.Api.V1.Employees
{
    [AllowAnonymous]
    [ApiRoute("employees")]
    public class EmployeeController : ApiController
    {
        #region Private Members

        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepositoryConductor _employeeConductor;

        #endregion Private Members

        #region Constructor

        public EmployeeController(
            IStringLocalizer localizer,
            IMapper mapper,
            ILogger<EmployeeController> logger,
            IEmployeeRepositoryConductor employeeConductor
        ) : base(localizer)
        {
            _mapper = mapper;
            _logger = logger;
            _employeeConductor = employeeConductor;
        }

        #endregion Constructor

        #region HTTP Get

        [HttpGet]
        public IActionResult Index()
        {
            var employeeResult = _employeeConductor.FindAll();
            if (employeeResult.HasErrors)
            {
                return InternalError<EmployeeDto>(null, employeeResult.Errors, _logger);
            }

            return Ok(_mapper.Map<List<EmployeeDto>>(employeeResult.ResultObject));

        }

        #endregion HTTP Get

    }
}