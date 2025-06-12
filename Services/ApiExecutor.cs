using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Smoker.Models;
using Smoker.Services;

namespace Smoker.Services
{
    public class ApiExecutor
    {
        private readonly VariableService _vars;
        private readonly TokenService _tokenSvc;
        private readonly List<TokenDefinition> _tokenDefs;
        private readonly StreamWriter _logger;

        public ApiExecutor(TokenService tokenSvc, List<TokenDefinition> tokenDefs, StreamWriter logger, VariableService vars)
            => (_tokenSvc, _tokenDefs, _logger, _vars) = (tokenSvc, tokenDefs, logger, vars);

        public async Task Execute(ApiDefinition def)
        {
            try
            {
                using var client = new HttpClient();

                // Replace variables in the URL and body using VariableService
                var url = _vars.ReplaceVariables(def.Endpoint);
                var body = _vars.ReplaceVariables(def.Payload ?? string.Empty);

                // Set Bearer token if needed
                if (def.IsTokenNeeded)
                {
                    var tokenDef = _tokenDefs.FirstOrDefault(x => x.Name == def.TokenGenerationAPIName);
                    if (tokenDef == null)
                        throw new Exception($"Token API '{def.TokenGenerationAPIName}' not found in token definitions.");

                    var token = await _tokenSvc.GetToken(tokenDef);
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Determine content type
                var contentType = string.IsNullOrWhiteSpace(def.ContentType)
                    ? "application/json"
                    : def.ContentType;

                // Execute the strategy (may include decorator for response parsing)
                var strategy = StrategyFactory.GetStrategy(def.Type);
                var response = await strategy.Execute(client, url, body, contentType);

                // Log response
                var log = $"""
                ==========================
                API: {def.Name}
                Endpoint: {url}
                Type: {def.Type}
                Status Code: {(int)response.StatusCode} - {response.StatusCode}
                Body:
                {await response.Content.ReadAsStringAsync()}
                """;

                await _logger.WriteLineAsync(log);
            }
            catch (Exception ex)
            {
                await _logger.WriteLineAsync($"[ERROR] {def.Name}: {ex.Message}");
            }
        }
    }
}
