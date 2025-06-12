using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Smoker.Services.Decorators
{
    public class ExtractOrderIdDecorator : HttpStrategyDecoratorBase
    {
        public ExtractOrderIdDecorator(IHttpStrategy inner) : base(inner) { }

        public override async Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType)
        {
            var response = await base.Execute(client, url, body, contentType);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("id", out var idProp))
                    {
                        var orderId = idProp.GetString();
                        if (!string.IsNullOrEmpty(orderId))
                        {
                            ValueStore.Set("OrderId", orderId);
                            Console.WriteLine($"[DEBUG] OrderId saved: {orderId}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to extract OrderId: {ex.Message}");
                }
            }

            return response;
        }
    }
   
}
