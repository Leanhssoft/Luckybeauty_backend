using Abp.Runtime.Caching;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.Storage
{
    public class TempFileCacheManager : ITempFileCacheManager
    {
        private const string TempFileCacheName = "TempFileCacheName";

        private readonly ITypedCache<string, TempFileInfo> _cache;

        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cache = cacheManager.GetCache<string, TempFileInfo>(TempFileCacheName);
        }

        public async void SetFile(string token, byte[] content)
        {
            await _cache.SetAsync(token, new TempFileInfo(content), TimeSpan.FromMinutes(5)); // expire time is 5 min by default
        }

        public byte[] GetFile(string token)
        {
            var cache = _cache.GetOrDefault(token);
            return cache?.File;
        }

        public async void SetFile(string token, TempFileInfo info)
        {
            await _cache.SetAsync(token, info, TimeSpan.FromMinutes(1)); // expire time is 1 min by default
        }

        public async Task<TempFileInfo> GetFileInfo(string token)
        {
           var file = await _cache.GetOrDefaultAsync(token);
           return file;
        }
        public async Task<string> GetFilePath(string token)
        {
            var fileInfo =await _cache.GetOrDefaultAsync(token);
            return fileInfo?.FileName;
        }
    }
}
