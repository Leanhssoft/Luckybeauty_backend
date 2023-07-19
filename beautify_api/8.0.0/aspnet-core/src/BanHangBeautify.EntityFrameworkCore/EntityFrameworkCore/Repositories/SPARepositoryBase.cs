using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using System.Data.Common;
using System.Data;

namespace BanHangBeautify.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class SPARepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<SPADbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected SPARepositoryBase(IDbContextProvider<SPADbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // Add your common methods for all repositories
        public DbCommand CreateCommand(string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            var command = GetConnection().CreateCommand();
            //command.CommandTimeout = 60 * 30;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetTransaction();
            return command;
        }
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="SPARepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class SPARepositoryBase<TEntity> : SPARepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        protected SPARepositoryBase(IDbContextProvider<SPADbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // Do not add any method here, add to the class above (since this inherits it)!!!
    }
    /// <summary>
    /// Base class for custom repositories of the application to only call store procedure
    /// </summary>
    public abstract class SPARepositoryBase : SPARepositoryBase<Entity, int>
    {
        protected SPARepositoryBase(IDbContextProvider<SPADbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
