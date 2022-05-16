using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models;
using AutoMapper;
using DylanJustice.Demo.Api.Business.Core.Interfaces.Providers;
using DylanJustice.Demo.Business.Core.Models.Dto;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;
using DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Providers
{
    public class EmployeeProvider : IEmployeeProvider
    {
        public const string ERR_GET_USERS_FAILED = "DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Providers.Employee.List.Unsuccessful";
        public const string ERR_UNHANDLED_EXCEPTION = "DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Providers.Employee.List.Exception";
        private readonly HttpClient _client;
        private readonly ILogger<EmployeeProvider> _logger;
        private readonly IMapper _mapper;
        public EmployeeProvider(HttpClient client, ILogger<EmployeeProvider> logger, IMapper mapper)
        {
            _client = client;
            _logger = logger;
            _mapper = mapper;
        }
        #region IProvider Implementation

        public bool Implemented => true;

        public string Name => "EmployeeProvider";

        #endregion IProvider Implementation

        #region IExternalUserProvider Implementation

        public async Task<IResult<IEnumerable<Employee>>> FindAll()
        {
            try
            {
                var listResponseMessage = await _client.GetAsync("/api/v1/users");
                if (!listResponseMessage.IsSuccessStatusCode)
                {
                    _logger.LogError($"Key: {ERR_GET_USERS_FAILED}, Message: Error occurred getting users. Response does not indicate success. ({listResponseMessage.StatusCode})");
                    return new Result<IEnumerable<Employee>>(ERR_GET_USERS_FAILED, $"Error occurred getting users. Response does not indicate success. ({listResponseMessage.StatusCode})");
                }
                var content = await listResponseMessage.Content.ReadAsStringAsync();
                var employeeGetResult = JsonConvert.DeserializeObject<Result<List<ExternalEmployeeDto>>>(content);
                if (employeeGetResult.HasErrors)
                {
                    return new Result<IEnumerable<Employee>>(ERR_GET_USERS_FAILED, employeeGetResult.ListErrors());
                }


                return new Result<IEnumerable<Employee>>(_mapper.Map<List<Employee>>(employeeGetResult.ResultObject));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<IEnumerable<Employee>>(ERR_GET_USERS_FAILED, ex.Message);
            }
        }

        #endregion IExternalUserProvider Implementation
    }
}