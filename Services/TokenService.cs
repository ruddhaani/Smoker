using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smoker.Models;

namespace Smoker.Services
{
    public class TokenService
    {
        private Dictionary<string, string> _tokens = new();

        public async Task<string> GetToken(TokenDefinition def)
        {
            if (_tokens.ContainsKey(def.Name))
                return _tokens[def.Name];

            using var client = new HttpClient();
            var content = new StringContent(def.Payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(def.Endpoint, content);
            var body = await response.Content.ReadAsStringAsync();
            var token = Newtonsoft.Json.Linq.JObject.Parse(body)[def.ResponseTokenKey]?.ToString();

            _tokens[def.Name] = token;
            return token;
        }
    }

}
