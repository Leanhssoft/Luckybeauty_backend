using BanHangBeautify.Debugging;

namespace BanHangBeautify
{
    public class SPAConsts
    {
        public const string LocalizationSourceName = "SPA";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;

        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 1000;
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "e384d1716a8642c28a9387892dd10535";
    }
}
