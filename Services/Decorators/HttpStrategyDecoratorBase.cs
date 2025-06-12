using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoker.Services.Decorators
{
    public abstract class HttpStrategyDecoratorBase : IHttpStrategy
    {
        protected readonly IHttpStrategy _inner;

        protected HttpStrategyDecoratorBase(IHttpStrategy inner)
        {
            _inner = inner;
        }

        public virtual async Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType)
        {
            return await _inner.Execute(client, url, body, contentType);
        }
    }
}
