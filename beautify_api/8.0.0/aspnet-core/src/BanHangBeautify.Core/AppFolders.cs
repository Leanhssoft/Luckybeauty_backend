using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }
        public string UploadFiles { get; set; }
    }
}
