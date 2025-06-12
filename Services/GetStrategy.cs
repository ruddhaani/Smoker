using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoker.Services
{
    public class GetStrategy : IHttpStrategy
    {
        public Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType)
        {
            return client.GetAsync(url);
        }
    }

}
