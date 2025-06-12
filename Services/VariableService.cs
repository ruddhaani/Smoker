using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Text.RegularExpressions;

namespace Smoker.Services
{
    public class VariableService
    {
        private readonly string _yamlPath;

        public VariableService(string yamlPath)
        {
            _yamlPath = yamlPath;
        }

        public string ReplaceVariables(string input)
        {
            // Replace from YAML
            var yaml = File.ReadAllText(_yamlPath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var config = deserializer.Deserialize<Dictionary<string, object>>(yaml);

            if (config.TryGetValue("Variables", out var varsObj) && varsObj is Dictionary<object, object> vars)
            {
                foreach (var entry in vars)
                {
                    input = input.Replace($"{{{{{entry.Key}}}}}", entry.Value.ToString());
                }
            }

            // Replace from in-memory ValueStore
            var matches = Regex.Matches(input, @"{{(.*?)}}");
            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value;
                var value = ValueStore.Instance.Get(key);
                if (value != null)
                {
                    input = input.Replace($"{{{{{key}}}}}", value);
                }
            }

            return input;
        }

        public void IncrementAndPersistVariables()
        {
            var input = File.ReadAllText(_yamlPath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var config = deserializer.Deserialize<Dictionary<string, object>>(input);

            if (config.TryGetValue("Variables", out var varsObj) && varsObj is Dictionary<object, object> vars)
            {
                var updatedVars = new Dictionary<string, string>();

                foreach (var entry in vars)
                {
                    var key = entry.Key.ToString();
                    var val = entry.Value.ToString();

                    if (long.TryParse(new string(val.Where(char.IsDigit).ToArray()), out long num))
                    {
                        var prefix = new string(val.Where(char.IsLetter).ToArray());
                        var newVal = prefix + (++num).ToString().PadLeft(val.Length - prefix.Length, '0');
                        updatedVars[key] = newVal;
                    }
                    else
                    {
                        updatedVars[key] = val;
                    }
                }

                config["Variables"] = updatedVars;

                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var output = serializer.Serialize(config);
                File.WriteAllText(_yamlPath, output);
            }
        }
    }
}
