using System.Collections.Generic;
using System.Threading.Tasks;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Interfaces.Providers;
using DylanJustice.Demo.Business.Core.Models.Dto;

namespace DylanJustice.Demo.Api.Business.Core.Interfaces.Providers
{
    public interface IEmployeeProvider : IProvider
    {
        Task<IResult<IEnumerable<Employee>>> FindAll();
    }
}