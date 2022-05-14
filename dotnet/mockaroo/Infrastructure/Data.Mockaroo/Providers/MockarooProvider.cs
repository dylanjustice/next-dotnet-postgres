
using AndcultureCode.CSharp.Core;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mockaroo.Business.Core.Interfaces;
using Mockaroo.Business.Core.Models.Users;
using Newtonsoft.Json;

namespace Mockaroo.Infrastructure.Data.Mockaroo.Providers
{
    public class MockarooProvider : IMockarooProvider
    {
        public const string ERR_GET_USERS_FAILED = "Mockaroo.Infrastructure.Data.Mockaroo.Providers.GetUsersResponse.Unsuccessful";
        private readonly HttpClient _client;
        private readonly string _baseUri;
        private readonly ILogger<MockarooProvider> _logger;
        public MockarooProvider(HttpClient client, IConfiguration _configuration, ILogger<MockarooProvider> logger)
        {
            _client = client;
            _baseUri = _configuration.GetValue<string>("MockarooBaseUrl");
            _logger = logger;
        }
        #region IProvider Implementation

        public bool Implemented => true;

        public string Name => "MockarooProvider";

        #endregion IProvider Implementation

        #region IMockarooProvider Implementation

        public async Task<List<User>> ListUsers()
        {
            try
            {
                var listResponseMessage = await _client.GetAsync(_baseUri);
                if (!listResponseMessage.IsSuccessStatusCode)
                {
                    _logger.LogError($"Key: {ERR_GET_USERS_FAILED}, Message: Error occurred getting users. Response does not indicate success. ({listResponseMessage.StatusCode})");
                }
                var users = JsonConvert.DeserializeObject<List<User>>(listResponseMessage.Content.ToString());

                return users;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new List<User>();
            }
        }

        #endregion IMockarooProvider Implementation

    }
}