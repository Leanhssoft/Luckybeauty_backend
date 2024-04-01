using Abp.Application.Services.Dto;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static Google.Apis.Drive.v3.FilesResource;

namespace BanHangBeautify.UploadFile
{
    public class GoogleAPIAppService : SPAAppServiceBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DriveService _service;
        private readonly IConfiguration _config;
        private readonly string _folderId;// Id của thư mục được chia sẻ trên google drive
        public GoogleAPIAppService(IWebHostEnvironment hostEnvironment, IConfiguration config)
        {
            _hostEnvironment = hostEnvironment;
            _config = config;
            _folderId = _config["GoogleApi:FolderShareId"];
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
        /// <summary>
        /// save file to server: return pathFile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
        private async Task<string> SaveFileToServer_toTemp([FromForm] IFormFile file)
        {
            try
            {
                string path = Path.GetTempPath();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, file.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    // Đọc dữ liệu từ IFormFile và ghi vào bộ nhớ đệm
                //    await file.CopyToAsync(memoryStream);

                //    // Chuyển đổi dữ liệu từ MemoryStream thành mảng byte
                //    byte[] imageData = memoryStream.ToArray();

                //    //return imageData;
                //}

                return path;
            }
            catch (Exception)
            {
                return string.Empty;
            }
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

        [HttpGet]
        public async Task<PagedResultDto<Google.Apis.Drive.v3.Data.File>> GetAllFile_ByProperties(string key, string value)
        {
            PagedResultDto<Google.Apis.Drive.v3.Data.File> data = new();
            ListRequest request = _service.Files.List();
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                request.Q = $"properties has {{ key='{key}' and value='{value}' }}";// remove all folder in tenantName
                request.Fields = "*";
                var result = await request.ExecuteAsync();
                data.Items = (IReadOnlyList<Google.Apis.Drive.v3.Data.File>)result.Files;
                data.TotalCount = result.Files.Count;
            }
            return data;
        }
        /// <summary>
        /// remove all file/ or list file in nameFolder: return true/false
        /// </summary>
        /// <param name="nameFolder"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> GoogleApi_RemoveFile_byNameFolder(string tenantName = null, string nameFolder = null)
        {
            try
            {
                ListRequest request = _service.Files.List();
                if (!string.IsNullOrEmpty(tenantName))
                {
                    request.Q = $"name = '{tenantName}' and properties has {{ key='roootFolder' and value='{_folderId}' }}";// remove all folder in tenantName
                    if (!string.IsNullOrEmpty(nameFolder))
                    {
                        // remove 1 nameFolder in tenantName
                        request.Q = $" name = '{nameFolder}' and properties has {{ key='tenantName' and value='{tenantName}' }}";
                    }
                }
                else
                {
                    // remove all nameFolder all tenantName
                    request.Q = $"name = '{nameFolder}' and properties has {{ key='roootFolder' and value='{_folderId}' }}";
                }
                request.Fields = "files(id)";
                var result = await request.ExecuteAsync();
                foreach (var item in result.Files)
                {
                    if (item.Id != _folderId)
                    {
                        _service.Files.Delete(item.Id).Execute();
                    }
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
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> GoogleApi_RemoveFile_byId(string fileId = null)
        {
            try
            {

                if (!string.IsNullOrEmpty(fileId))
                {
                    var request = _service.Files.Get(fileId);
                    request.Fields = "*";
                    var result = await request.ExecuteAsync();
                    _service.Files.Delete(result.Id).Execute();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// return idFolder/idSubFolder
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GoogleApi_CheckExistFolder(string tenantName, string subFolder = null)
        {
            string idSubFolder = string.Empty;
            try
            {
                var request = _service.Files.List();
                // tìm kiếm thư mục có tên = tenantName
                request.Q = $"mimeType = 'application/vnd.google-apps.folder' and name='{tenantName}'";
                request.Fields = "files(id)";
                var result = await request.ExecuteAsync();
                if (result.Files.Count > 0)
                {
                    // check exist subFolder
                    if (!string.IsNullOrEmpty(subFolder))
                    {
                        var rqSubFolder = _service.Files.List();
                        rqSubFolder.Q = $"mimeType = 'application/vnd.google-apps.folder' and '{result.Files[0].Id}' in parents and name='{subFolder}' and properties has {{ key='tenantName' and value='{tenantName}' }}";
                        rqSubFolder.Fields = "files(id)";
                        var lstSubFolder = await rqSubFolder.ExecuteAsync();

                        if (lstSubFolder.Files.Count > 0)
                        {
                            idSubFolder = lstSubFolder.Files[0].Id;
                        }
                        else
                        {
                            // create subfolder
                            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                            {
                                Name = subFolder,
                                Parents = new[] { result.Files[0].Id },// Parents: id of tenantName
                                MimeType = "application/vnd.google-apps.folder",
                                Properties = new Dictionary<string, string>
                                {
                                    { "roootFolder", _folderId },
                                    { "tenantName", tenantName },
                                }
                            };
                            var createSub = _service.Files.Create(fileMetadata);
                            createSub.Fields = "id";// định nghĩa các trường sẽ dc trả về khi request (*: return all, Id: return Id)
                            idSubFolder = createSub.Execute().Id;
                        }
                    }
                    else
                    {
                        idSubFolder = result.Files[0].Id;
                    }
                }
                else
                {
                    // tạo thư mục mới (thuộc folder share in drive) có tên = tenantName
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = tenantName,
                        Parents = new[] { _folderId },
                        MimeType = "application/vnd.google-apps.folder",
                        Properties = new Dictionary<string, string>
                        {
                            { "roootFolder", _folderId },
                        }
                    };
                    var rqTenantName = _service.Files.Create(fileMetadata);
                    rqTenantName.Fields = "id";

                    // create subfolder
                    var fileMetadata2 = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = subFolder,
                        Parents = new[] { rqTenantName.Execute().Id },// Parents: id of tenantName
                        MimeType = "application/vnd.google-apps.folder",
                        Properties = new Dictionary<string, string>
                        {
                            { "roootFolder", _folderId },
                            { "tenantName", tenantName }
                        }
                    };
                    var rqSubFolder = _service.Files.Create(fileMetadata2);
                    rqSubFolder.Fields = "id";// định nghĩa các trường sẽ dc trả về khi request (*: return all, Id: return Id)
                    idSubFolder = rqSubFolder.Execute().Id;
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
        /// <param name="file"></param>
        /// <param name="tenantName"></param>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GoogleApi_UploaFileToDrive([FromForm] IFormFile file, string tenantName, string subFolder = null)
        {
            try
            {
                string fileId = "";
                if (file != null && file.Length > 0)
                {
                    var mimeType = GetMimeType(file.FileName);
                    string folderId = await GoogleApi_CheckExistFolder(tenantName, subFolder);

                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = file.FileName,
                        Parents = new[] { folderId },
                        MimeType = mimeType
                    };

                    string path = await SaveFileToServer_toTemp(file);

                    FilesResource.CreateMediaUpload request;
                    // Create a new file, with metadata and stream.
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        request = _service.Files.Create(
                            fileMetadata, stream, mimeType);
                        request.Fields = "id";
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
