using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smoker.Services.Decorators;

namespace Smoker.Services
{
    public static class StrategyFactory
    {
        public static IHttpStrategy GetStrategy(string type)
        {
            return type.ToUpper() switch
            {
                "GET" => new GetStrategy(),
                "POST" => new ExtractOrderIdDecorator(new PostStrategy()),
                "PUT" => new PutStrategy(),
                "FILEUPLOAD" => new FileUploadStrategy(),
                _ => throw new NotSupportedException($"HTTP method {type} is not supported."),
            };
        }
    }

}
