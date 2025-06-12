namespace Smoker.Models
{
    public class AppConfig
    {
        public Dictionary<string, string> Variables { get; set; }
        public List<TokenDefinition> TokenGenerators { get; set; }
        public List<ApiDefinition> APIs { get; set; }
    }
}
