using Microsoft.AspNetCore.Hosting;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Google.Apis.Upload;
using NPOI.OpenXmlFormats.Wordprocessing;
using Microsoft.AspNetCore.StaticFiles;
using ICSharpCode.SharpZipLib.Core;
using Google.Apis.Drive.v3.Data;
using Nito.AsyncEx;
using System.Linq;
using NPOI.HSSF.Record.PivotTable;

namespace BanHangBeautify.UploadFile
{
    public class GoogleAPIAppService : SPAAppServiceBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DriveService _service;
        private const string _folderId = "1m1VSdF9sP435_wCStZyhLSoqCGhlzu7J";// Id của thư mục được chia sẻ trên google drive
        public GoogleAPIAppService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            // cấp quyền truy cập vào drive
            var pathFile = Path.Combine(_hostEnvironment.WebRootPath, @"GoogleAPI\credentials.json");
            var credential = GoogleCredential.FromFile(pathFile).CreateScoped(DriveService.ScopeConstants.Drive);
            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Lucky beauty",
            });
        }

        private string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        public async Task<IList<Google.Apis.Drive.v3.Data.File>> GoogleApi_GetAllFile()
        {
            var lst = _service.Files.List();
            var result = await lst.ExecuteAsync();
            foreach (var item in result.Files)
            {
                Console.WriteLine($"Parents: {item.Parents} - fileId: {item.Id} - fileName: {item.Name}");
            }
            return result.Files;
        }
        /// <summary>
        /// Update the file id, with new metadata and stream.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<bool> GoogleApi_UpdateFileIfExist(string fileId, string fileName)
        {
            try
            {
                var mimeType = GetMimeType(fileName);
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                };
                await using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                var updateRequest = _service.Files.Update(fileMetadata, fileId, stream, mimeType);
                var result = await updateRequest.UploadAsync(CancellationToken.None);

                if (result.Status == UploadStatus.Failed)
                {
                    Console.WriteLine($"Error uploading file: {result.Exception.Message}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public async Task<string> GoogleApi_CheckExistFolder(string tenantName)
        {
            string idSubFolder = string.Empty;
            try
            {
                var request = _service.Files.List();
                // tìm kiếm thư mục có tên = tenantName
                request.Q = $"mimeType = 'application/vnd.google-apps.folder' and name={tenantName}";
                var result = await request.ExecuteAsync();
                if (result.Files.Count > 0) {
                    idSubFolder = result.Files[0].Id;
                }
                else
                {
                    // tạo thư mục mới (thuộc folder share in drive)
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = tenantName,
                        Parents = new[] { _folderId },
                        MimeType = "application/vnd.google-apps.folder"
                    };
                    var request2 = _service.Files.Create(fileMetadata);
                    request2.Fields = "*";// định nghĩa các trường sẽ dc trả về khi request (*: return all, Id: return Id)
                    idSubFolder = request2.Execute().Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"GoogleApi_CheckExistFolder: {e.Message}");
            }
            return idSubFolder;
        }
        /// <summary>
        /// upload file to drive --> return fileId
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathFile"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public async Task<string> GoogleApi_UploaFileToDrive(string fileName, string pathFile, string tenantName)
        {
            try
            {
                var mimeType = GetMimeType(fileName);
                string folderId = await GoogleApi_CheckExistFolder(tenantName);

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    Parents = new[] { folderId },
                    MimeType = mimeType
                };
                FilesResource.CreateMediaUpload request;
                // Create a new file, with metadata and stream.
                using (var stream = new FileStream(pathFile, FileMode.Open))
                {
                    request = _service.Files.Create(
                        fileMetadata, stream, mimeType);
                    request.Fields = "*";
                    request.Upload();
                }
                return request.ResponseBody?.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading file: {e.Message}");
                return string.Empty;
            }
        }
    }
}
