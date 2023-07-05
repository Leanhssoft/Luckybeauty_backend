using Abp.Auditing;
using Abp.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Controllers
{
    public class FileController: SPAControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IMimeTypeMap mimeTypeMap
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}
