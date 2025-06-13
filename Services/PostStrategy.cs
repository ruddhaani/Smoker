using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoker.Services
{
    public class PostStrategy : IHttpStrategy
    {
        public Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType)
        {
            var content = new StringContent(body, Encoding.UTF8, contentType);
            return client.PostAsync(url, content);
        }
    }

}
