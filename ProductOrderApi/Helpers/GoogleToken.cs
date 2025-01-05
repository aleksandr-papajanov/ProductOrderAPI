using Newtonsoft.Json;

namespace ProductOrderApi.Helpers
{
    internal class GoogleToken
    {
        private readonly DateTime _createdAt = DateTime.Now;

        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonProperty("id_token")]
        public string IdToken { get; set; } = null!;

        public string RequestType { get; set; } = null!;

        public DateTime ExpiresAt => _createdAt.AddSeconds(ExpiresIn);
        public bool IsExpired => DateTime.Now > ExpiresAt;
    }
}
