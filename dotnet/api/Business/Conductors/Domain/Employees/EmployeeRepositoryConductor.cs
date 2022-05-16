using System.Collections.Generic;
using AndcultureCode.CSharp.Core;
using AndcultureCode.CSharp.Core.Interfaces;
using DylanJustice.Demo.Api.Business.Core.Interfaces.Providers;
using DylanJustice.Demo.Business.Core.Interfaces.Conductors.Domain.Employees;
using DylanJustice.Demo.Business.Core.Models.Dto;
using Microsoft.Extensions.Logging;

namespace DylanJustice.Demo.Business.Conductors.Domain.Employees
{
    public class EmployeeRepositoryConductor : IEmployeeRepositoryConductor
    {
        #region Private Members

        private readonly ILogger<EmployeeRepositoryConductor> _logger;
        private readonly IEmployeeProvider _employeeProvider;

        #endregion Private Members

        #region Constructor

        public EmployeeRepositoryConductor(
            ILogger<EmployeeRepositoryConductor> logger,
            IEmployeeProvider employeeProvider
        )
        {
            _logger = logger;
            _employeeProvider = employeeProvider;
        }

        #endregion Constructor

        #region IEmployeeRepositoryConductor

        public IResult<IEnumerable<Employee>> FindAll() => Do<IEnumerable<Employee>>.Try((r) =>
        {
            var employees = _employeeProvider.FindAll().Result;

            return employees;
        }).Result;

        #endregion IEmployeeRepositoryConductor
    }
}