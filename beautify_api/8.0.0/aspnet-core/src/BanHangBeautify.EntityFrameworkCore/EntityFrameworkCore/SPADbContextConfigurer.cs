using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BanHangBeautify.EntityFrameworkCore
{
    public static class SPADbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SPADbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SPADbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
