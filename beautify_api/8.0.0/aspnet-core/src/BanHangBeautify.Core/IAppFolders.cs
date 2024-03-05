namespace BanHangBeautify
{
    public interface IAppFolders
    {
        string SampleProfileImagesFolder { get; }

        string WebLogsFolder { get; set; }
        string UploadFiles { get; set; }
    }
}
