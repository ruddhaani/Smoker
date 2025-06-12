using Smoker.Models;
using Smoker.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static async Task Main(string[] args)
    {
        string basePath = AppContext.BaseDirectory;
        //string configPath = Path.Combine(basePath, "Configs", "apis.yaml");
        string configPath = Path.Combine("C:\\POC\\Smoker\\Smoker\\Configs\\apis.yaml");


        var yaml = File.ReadAllText(configPath);

        var deserializer = new DeserializerBuilder()
                            .WithNamingConvention(PascalCaseNamingConvention.Instance)
                            .Build();
        var config = deserializer.Deserialize<AppConfig>(yaml);

        var logger = LoggerFactory.CreateLogger();
        var varSvc = new VariableService("C:\\POC\\Smoker\\Smoker\\Configs\\apis.yaml");
        var tokenSvc = new TokenService();
        var executor = new ApiExecutor(tokenSvc, config.TokenGenerators, logger , varSvc);

        foreach (var api in config.APIs)
            await executor.Execute(api);

        varSvc.IncrementAndPersistVariables();
        await logger.FlushAsync();
        logger.Close();

        Console.WriteLine("Smoke Test was completed. Have a nice and automated day. Don't forget to check those logs!");
        Console.ReadKey();
    }
}
