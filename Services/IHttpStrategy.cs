using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoker.Services
{
    public interface IHttpStrategy
    {
        Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType);
    }

}
