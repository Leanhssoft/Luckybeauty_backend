using BanHangBeautify.Debugging;
using System;

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
        //public static readonly string DefaultPassPhrase =
        //    DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "e384d1716a8642c28a9387892dd10535";
        public static readonly string DefaultPassPhrase = "e384d1716a8642c28a9387892dd10535";
        public const string TokenValidityKey = "token_validity_key";
        public const string RefreshTokenValidityKey = "refresh_token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";

        public const string TokenType = "token_type";

        public static string UserIdentifier = "user_identifier";
        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);

        public const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";
        //public static readonly string DefaultPassPhrase = "e384d1716a8642c28a9387892dd10535";
    }
}
