using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.Web.Models;
using BanHangBeautify.Controllers;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace BanHangBeautify.Web.Host.Controllers
{
    [AbpMvcAuthorize]
    [Route("api/upload-file")]
    [DontWrapResult]
    public class UploadController : SPAControllerBase
    {
        private readonly IAppFolders _appFolders;
        private readonly IHostingEnvironment _env;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public UploadController(IHostingEnvironment env, IAppFolders appFolders,
            ITempFileCacheManager tempFileCacheManager,
            IBackgroundJobManager backgroundJobManager)
        {
            _env = env;
            _appFolders = appFolders;
            _backgroundJobManager = backgroundJobManager;
            _tempFileCacheManager = tempFileCacheManager;
        }
        [HttpGet]
        [Route("download-import-template")]
        public async Task<ActionResult<FileDto>> DownloadImportTemplate(string fileName)
        {
            var fullPath = Path.Combine(_env.WebRootPath, $"ImportExcelTemplate", fileName);
            var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);

            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            _tempFileCacheManager.SetFile(file.FileToken, bytes);

            return file;
        }

    }
}
