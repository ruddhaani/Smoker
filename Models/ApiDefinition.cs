using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoker.Models
{
    public class ApiDefinition
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Endpoint { get; set; }
        public string Payload { get; set; }
        public bool IsTokenNeeded { get; set; }
        public string TokenGenerationAPIName { get; set; }

        public string ContentType { get; set; } = "application/json";
    }

}
