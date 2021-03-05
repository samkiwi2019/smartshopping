using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Smartshopping.Data;

namespace Smartshopping.Controllers
{
    public abstract class CommonController : ControllerBase
    {
        
        private async Task<string> GetRawBodyStringAsync(HttpRequest request)
        {
            using var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }
    
        protected async Task<SearchParams> GetSearchParams(HttpRequest request)
        {
            var rawValue = await GetRawBodyStringAsync(request);
            if (string.IsNullOrEmpty(rawValue)) return new SearchParams();
            var serializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            return JsonConvert.DeserializeObject<SearchParams>(rawValue, serializerSettings);
        }
    }
}