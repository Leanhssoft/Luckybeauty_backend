using Abp.Dependency;

namespace BanHangBeautify
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }
        public string UploadFiles { get; set; }
    }
}
