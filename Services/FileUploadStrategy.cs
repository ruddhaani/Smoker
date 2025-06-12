using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Smoker.Models;
using Smoker.Services;

namespace Smoker.Strategies
{
    public class FileUploadStrategy : IHttpStrategy
    {
        public async Task<HttpResponseMessage> Execute(HttpClient client, string url, string body, string contentType)
        {
            Console.WriteLine("[FileUploadStrategy] Entering Execute()");

            FileUploadPayload payload;
            try
            {
                payload = JsonSerializer.Deserialize<FileUploadPayload>(body);
                Console.WriteLine("[FileUploadStrategy] Deserialized payload successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FileUploadStrategy] Payload deserialization failed: {ex.Message}");
                throw;
            }

            if (!File.Exists(payload.FilePath))
            {
                Console.WriteLine($"[FileUploadStrategy] File not found: {payload.FilePath}");
                throw new FileNotFoundException("Upload file not found.", payload.FilePath);
            }

            var boundary = "----SmokerBoundary" + Guid.NewGuid().ToString("N");
            var content = new MultipartFormDataContent(boundary);

            // Force boundary formatting like Postman
            content.Headers.ContentType = MediaTypeHeaderValue.Parse($"multipart/form-data; boundary={boundary}");

            var fileFieldName = string.IsNullOrWhiteSpace(payload.FileFieldName) ? "file" : payload.FileFieldName;
            var fileName = payload.FileName ?? Path.GetFileName(payload.FilePath);

            // Do not read or buffer file, stream directly
            var fileStream = new FileStream(payload.FilePath, FileMode.Open, FileAccess.Read);

            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

            content.Add(fileContent, fileFieldName, fileName);
            Console.WriteLine($"[FileUploadStrategy] File part added: field={fileFieldName}, name={fileName}");

            if (!string.IsNullOrEmpty(payload.Description))
                content.Add(new StringContent(payload.Description), "Description");

            if (!string.IsNullOrEmpty(payload.Type))
                content.Add(new StringContent(payload.Type), "Type");

            if (!string.IsNullOrEmpty(payload.FileName))
                content.Add(new StringContent(payload.FileName), "FileName");

            Console.WriteLine("[FileUploadStrategy] Metadata fields added");

            HttpResponseMessage response;
            try
            {
                Console.WriteLine("[FileUploadStrategy] Sending POST...");
                response = await client.PostAsync(url, content);
                Console.WriteLine($"[FileUploadStrategy] Response received: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FileUploadStrategy] HTTP POST failed: {ex.Message}");
                throw;
            }
            finally
            {
                fileStream.Dispose();
                Console.WriteLine("[FileUploadStrategy] File stream disposed");
            }

            return response;
        }

        private class FileUploadPayload
        {
            public string FileFieldName { get; set; }
            public string FilePath { get; set; }
            public string Description { get; set; }
            public string FileName { get; set; }
            public string Type { get; set; }
        }
    }
}
