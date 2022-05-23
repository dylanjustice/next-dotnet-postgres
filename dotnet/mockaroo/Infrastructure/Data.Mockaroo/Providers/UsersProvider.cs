
using AndcultureCode.CSharp.Core;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models.Errors;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mockaroo.Business.Core.Interfaces;
using Mockaroo.Business.Core.Models.Users;
using Mockaroo.Infrastructure.Data.Mockaroo.Models;
using Newtonsoft.Json;

namespace Mockaroo.Infrastructure.Data.Mockaroo.Providers
{
    public class UsersProvider : IUsersProvider
    {
        public const string ERR_GET_USERS_FAILED = "Mockaroo.Infrastructure.Data.Mockaroo.Providers.GetUsersResponse.Unsuccessful";
        public const string UNHANDLED_EXCEPTION = "Mockaroo.Infrastructure.Data.Mockaroo.Providers.GetUsersResponse.Exception";
        private readonly HttpClient _client;
        private readonly ILogger<UsersProvider> _logger;
        private readonly IMapper _mapper;
        public UsersProvider(HttpClient client, ILogger<UsersProvider> logger, IMapper mapper)
        {
            _client = client;
            _logger = logger;
            _mapper = mapper;
        }
        #region IProvider Implementation

        public bool Implemented => true;

        public string Name => "MockarooProvider";

        #endregion IProvider Implementation

        #region IMockarooProvider Implementation

        public async Task<IResult<List<User>>> ListUsers()
        {
            try
            {
                var listResponseMessage = await _client.GetAsync("/users");
                if (!listResponseMessage.IsSuccessStatusCode)
                {
                    return new Result<List<User>>(ERR_GET_USERS_FAILED, $"Message: Error occurred getting users. Response does not indicate success. ({listResponseMessage.StatusCode})");
                }
                var content = await listResponseMessage.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<MockarooUserDto>>(content);

                return new Result<List<User>>(_mapper.Map<List<User>>(users));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<List<User>>(UNHANDLED_EXCEPTION, ex.Message);
            }
        }

        #endregion IMockarooProvider Implementation

    }
}