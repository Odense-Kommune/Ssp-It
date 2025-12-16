using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Xflow.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Dk.Odense.SSP.Core.Enum;
using System.Text;
using Microsoft.Extensions.Options;

namespace Dk.Odense.SSP.Xflow.Services
{
    public class BaseXFlowService<TValue, TRepository> : IBaseXFlowService<TValue> where TRepository: IBaseXFlowRepository<TValue>
                                                                            where TValue : class, IEntity, new()
    {
        private readonly TRepository repository;
        private readonly XFlowConfig _xFlowConfig;
        private readonly HttpClient _httpClient;

        public BaseXFlowService(TRepository repository, IOptions<XFlowConfig> xFlowConfig)
        {
            this.repository = repository;
            _xFlowConfig = xFlowConfig.Value;
            _httpClient = CreateClient();
        }

        public virtual async Task<List<TValue>?> Create(List<TValue> values)
        {
            return await repository.Create(values);
        }

        public virtual async Task<List<TValue>> List()
        {
            return await repository.List();
        }

        public virtual async Task<bool> FindDuplicate(Guid id)
        {
            return await repository.FindDuplicate(id);
        }

        public async Task<IEnumerable<JToken>?> GetFormIds(string[] publicId)
        {
            string jsonBody = JsonConvert.SerializeObject(new
            {
                processTemplateIds = publicId,
            });

            StringContent jsonContent = new(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Process/Search", jsonContent);

            var result = await GetJSONObject<IEnumerable<JToken>>(response);
            return result;
        }

        public static async Task<T?> GetJSONObject<T>(HttpResponseMessage response)
        {
            int maxRetries = 3;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    response.EnsureSuccessStatusCode(); // Ensure that the response indicates success
                    //string responseString = await response.Content.ReadAsStringAsync(); //only relevant for debugging the response
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        // Log errorContent for debugging
                        throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
                    }

                    using Stream responseStream = await response.Content.ReadAsStreamAsync();
                    var output = "";
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        output = reader.ReadToEnd();
                    }
                    T? result = JsonConvert.DeserializeObject<T>(output);
                    return result;
                }
                catch (HttpRequestException) when (i < maxRetries - 1)
                {
                    await Task.Delay(1000 * (i + 1)); // Exponential backoff
                }
            }
            throw new HttpRequestException("Request failed after retries.");
        }

        public async Task<JToken?> GetFormsFromXFlow(Guid publicId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Process/" + publicId);
            var result = await GetJSONObject<JToken>(response);
            return result;
        }

        public Assessment? MapAssessment(JToken form)
        {
            var assessment = new Assessment();
            foreach (var property in assessment.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(Status))
                {
                    string identifier = property.Name;
                    var element = form.Value<JArray>("elementer")?.FirstOrDefault(x => x.Value<string>("identifier") == identifier);
                    var enumStatus = MapProperty(element);
                    assessment.GetType().GetProperty(identifier)?.SetValue(assessment, enumStatus);

                    string elaborationName = identifier + "Elaborate";
                    var elaboration = MapPropertyElaboration(element);
                    assessment.GetType().GetProperty(elaborationName)?.SetValue(assessment, elaboration);
                }
            }
            return assessment;
        }

        public static Status MapProperty(JToken? element)
        {
            var value = element?.Value<JObject>("values")?.Value<string>("MultiSelectTekst") ?? ""; //Should never return an empty string, but if so, maps to yellow
            return value switch
            {
                "Rød" => Status.Red,
                "Gul" => Status.Yellow,
                "Grøn" => Status.Green,
                _ => Status.Yellow,
            };
        }

        public string MapPropertyElaboration(JToken? element)
        {
            if (element == null) return "";
            var elaboration = element
                .Value<JArray>("children")?.FirstOrDefault(x => x.Value<string>("identifier")?.StartsWith("ElementContainer") == true)?
                .Value<JArray>("children")?.FirstOrDefault(y => y.Value<string>("identifier")?.StartsWith("ElementTextfield") == true)?
                .Value<JObject>("values")?.Value<string>("Tekst");
            return ReplaceChars(elaboration);
        }

        public string ReplaceChars(string? str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            str = str.Replace("\"", "");
            str = str.Replace("'", "");
            return str;
        }

        public static string FormatSocialSecNum(string? str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            str = str.Replace("-", "");
            return str;
        }

        public string? GetChildFromField(JToken? obj, string identifier, string fieldName)
        {
            return obj?
                .FirstOrDefault(x => x.Value<string>("identifier") == identifier)?
                .Value<JObject>("values")?.Value<string>(fieldName);
        }

        public HttpClient CreateClient()
        {
            HttpClient httpClient = new()
            {
                BaseAddress = new Uri(_xFlowConfig.XFlowBaseURI),
                DefaultRequestHeaders = { { _xFlowConfig.TokenName, _xFlowConfig.TokenValue } }
            };
            return httpClient;
        }

    }
}
