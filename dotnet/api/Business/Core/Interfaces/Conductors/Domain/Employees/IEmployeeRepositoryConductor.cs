using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces;
using DylanJustice.Demo.Business.Core.Models.Dto;

namespace DylanJustice.Demo.Business.Core.Interfaces.Conductors.Domain.Employees
{
    public interface IEmployeeRepositoryConductor
    {
        public IResult<IEnumerable<Employee>> FindAll();

    }
}