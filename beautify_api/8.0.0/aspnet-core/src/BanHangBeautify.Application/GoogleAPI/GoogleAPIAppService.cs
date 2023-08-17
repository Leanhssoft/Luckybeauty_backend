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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using BanHangBeautify.NewFolder;
using Microsoft.AspNetCore.Http;
using NPOI.HPSF;

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
        [HttpGet]
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
        /// update file if exists/ else insert
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileId"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GoogleApi_UpdateFileIfExist([FromForm] IFormFile file, string fileId, string tenantName)
        {
            try
            {
                string fileIdReturn = string.Empty;
                if (file.Length > 0)
                {
                    var mimeType = GetMimeType(file.FileName);
                    // check exists fileId exist in folder tenantName in google drive
                    var request = _service.Files.List();
                    request.Q = $"'{fileId}' in {tenantName}";
                    var resultFile = await request.ExecuteAsync();

                    string pathFileNew = await SaveFileToServer(file);
                  
                    if (resultFile.Files.Count > 0)
                    {
                        // update
                        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                        {
                            Name = file.FileName,
                        };

                        await using var stream = new FileStream(pathFileNew, FileMode.Open, FileAccess.Read);
                        var updateRequest = _service.Files.Update(fileMetadata, fileId, stream, mimeType);
                        var result = await updateRequest.UploadAsync(CancellationToken.None);
                        if (result.Status == UploadStatus.Failed)
                        {
                            Console.WriteLine($"Error uploading file: {result.Exception.Message}");
                        }
                        fileIdReturn = fileId;
                    }
                    else
                    {
                        // insert
                        fileIdReturn = await GoogleApi_UploaFileToDrive(file, tenantName);
                    }
                }
                return fileIdReturn;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GoogleApi_CheckExistFolder(string tenantName)
        {
            string idSubFolder = string.Empty;
            try
            {
                var request = _service.Files.List();
                // tìm kiếm thư mục có tên = tenantName
                request.Q = $"mimeType = 'application/vnd.google-apps.folder' and name='{tenantName}'";
                var result = await request.ExecuteAsync();
                if (result.Files.Count > 0)
                {
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

        private async Task<string> SaveFileToServer([FromForm] IFormFile file)
        {
            try
            {
                // save file to server use Buffering: used to get path upload file to drive
                string path = Path.Combine(_hostEnvironment.WebRootPath, "UploadedFiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, file.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return path;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// upload file to drive --> return fileId
        /// </summary>
        /// <param name="file:IFormFile"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GoogleApi_UploaFileToDrive([FromForm] IFormFile file, string tenantName)
        {
            try
            {
                string fileId = "";
                if (file.Length > 0)
                {
                    string path = "";
                    var mimeType = GetMimeType(file.FileName);
                    string folderId = await GoogleApi_CheckExistFolder(tenantName);

                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = file.FileName,
                        Parents = new[] { folderId },
                        MimeType = mimeType
                    };

                    // save file to server use Buffering: used to get path upload file to drive
                    path = Path.Combine(_hostEnvironment.WebRootPath, "UploadedFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, file.FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    FilesResource.CreateMediaUpload request;
                    // Create a new file, with metadata and stream.
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        request = _service.Files.Create(
                            fileMetadata, stream, mimeType);
                        request.Fields = "*";
                        request.Upload();
                    }
                    fileId = request.ResponseBody?.Id;

                    // remove file after upload to drive
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                return fileId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading file: {e.Message}");
                return string.Empty;
            }
        }
    }
}
