using System.Text.Json;

namespace Smoker.Services
{
    public class FileUploadStrategy : IHttpStrategy
    {
        public async Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType)
        {
            var content = new MultipartFormDataContent();

            // Parse payload JSON to extract form fields and file path
            var payloadDict = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            // Required: file field name (e.g., "file") and file path
            string fileFieldName = payloadDict.TryGetValue("fileFieldName", out var fieldName) ? fieldName : "file";
            string filePath = payloadDict.TryGetValue("filePath", out var path) ? path : "Configs/testfile.txt";

            // Add other key-value pairs as form fields
            foreach (var kvp in payloadDict.Where(k => k.Key != "filePath" && k.Key != "fileFieldName"))
            {
                content.Add(new StringContent(kvp.Value), kvp.Key);
            }

            // Add file to the multipart content
            if (File.Exists(filePath))
            {
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                content.Add(new ByteArrayContent(fileBytes), fileFieldName, Path.GetFileName(filePath));
            }
            else
            {
                throw new FileNotFoundException($"Upload file not found: {filePath}");
            }

            return await client.PostAsync(url, content);
        }
    }
}
