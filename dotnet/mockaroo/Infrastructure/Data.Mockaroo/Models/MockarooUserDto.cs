using Newtonsoft.Json;

namespace Mockaroo.Infrastructure.Data.Mockaroo.Models
{
    public class MockarooUserDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }

    }
}