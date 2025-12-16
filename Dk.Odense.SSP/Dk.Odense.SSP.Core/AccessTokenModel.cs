using System.Text.Json.Serialization;

namespace Dk.Odense.SSP.Core
{
    public class AccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}