using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace FractalPlatform.Deployment
{
    class Program
    {
        const string _deploymentPath = @"..\..\..\..\";

        static string ZipDirectory(string directoryName, string appName)
        {
            string startPath = @$"{_deploymentPath}\BigDoc.App\{directoryName}\{appName}";
            
            if (Directory.Exists(startPath))
            {
                string zipPath = @$"{_deploymentPath}\BigDoc.Deployment\{directoryName}_{appName}.zip";

                if(File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                ZipFile.CreateFromDirectory(startPath, zipPath);

                return zipPath;
            }

            return null;
        }

        private static async Task UploadAsync(string baseUrl,
                                         string appName,
                                         string fileType,
                                         string fileName,
                                         byte[] fileBytes,
                                         string deploymentKey)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(fileBytes)), "upload", fileName);

                    var url = $"{baseUrl}/Home/UploadFile?appName={appName}&fileType={fileType}&deploymentKey={deploymentKey}";

                    var response = await client.PostAsync(url, content);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Wrong status code: {response.StatusCode}");
                    }
                }
            }
        }

        private static async Task UploadAsync(string baseUrl,
                                              string appName,
                                              string assemblyName,
                                              string deploymentKey,
                                              bool isDeployDatabase,
                                              bool isDeployApplication)
        {
            if (isDeployDatabase)
            {
                //upload database
                var zipPath = ZipDirectory("Databases", appName);

                if (zipPath != null)
                {
                    var fileBytes = await File.ReadAllBytesAsync(zipPath);

                    await UploadAsync(baseUrl, appName, "Databases", $"{appName}.zip", fileBytes, deploymentKey);
                }
            }

            if (isDeployApplication)
            {
                //upload layouts
                var zipPath = ZipDirectory("Layouts", appName);

                if (zipPath != null)
                {
                    var fileBytes = await File.ReadAllBytesAsync(zipPath);

                    await UploadAsync(baseUrl, appName, "Layouts", $"{appName}.zip", fileBytes, deploymentKey);
                }

                //upload assembly
#if DEBUG
                var filePath = @$"{_deploymentPath}\BigDoc.App\bin\Debug\netcoreapp3.1\{assemblyName}";
#else
            var filePath = @$"{_deploymentPath}\BigDoc.App\bin\Release\netcoreapp3.1\{assemblyName}";
#endif

                if (File.Exists(filePath))
                {
                    var fileBytes = await File.ReadAllBytesAsync(filePath);

                    await UploadAsync(baseUrl, appName, "Assembly", assemblyName, fileBytes, deploymentKey);
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new Exception("Command line: BigDoc.Deployment.exe [baseUrl] [appName] [assemblyName]");
            }

            var baseUrl = args[0];

            var appName = args[1];

            var assemblyName = args[2];

            var isDeployDatabase = true;

            var isDeployApplication = true;

            var deploymentKey = "sandbox";

            var isRunBrowser = true;

            Console.WriteLine($"Start deploying {appName} application to {baseUrl} host ...");

            UploadAsync(baseUrl, appName, assemblyName, deploymentKey, isDeployDatabase, isDeployApplication).Wait();

            Console.WriteLine("Application is deployed !");

            if (isRunBrowser)
            {
                var url = string.Format($"{baseUrl}?appName={appName}");

                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

            }
        }
    }
}
