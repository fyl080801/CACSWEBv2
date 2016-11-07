using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACSLibrary.Component;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace CACS.Framework.Data
{
    public class CACSWebObjectContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>, IDbContext
    {
        static readonly ReaderWriterLockSlim LOCKER = new ReaderWriterLockSlim();
        ITransaction _transaction;

        public ITransaction Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    using (new WriteLocker(LOCKER))
                    {
                        if (_transaction == null)
                            _transaction = new EfTransaction(this.Database, this);
                    }
                }
                return _transaction;
            }
        }

        public CACSWebObjectContext(string connectionString)
            : base(connectionString)
        { }

        public CACSWebObjectContext()
            : base(new Func<string>(delegate
                {
                    var settings = EngineContext.Current.Resolve<IDataSettingsManager>().LoadSettings();
                    if (string.IsNullOrEmpty(settings.ConnectionString))
                    {
                        return @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MSSQLLocalDB.mdf;Initial Catalog=CACS;Integrated Security=True";
                    }
                    return settings.ConnectionString;
                })
            .Invoke())
        {
        }

        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public int ExecuteSqlCommand(string sql, int? timeout = default(int?), params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var result = this.Database.ExecuteSqlCommand(sql, parameters);

            if (timeout.HasValue)
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseObjectEntity, new()
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("存储过程包含不支持的参数类型");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseObjectEntity
        {
            return base.Set<TEntity>();
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseObjectEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                return alreadyAttached;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var settings = EngineContext.Current.Resolve<IDataSettingsManager>().LoadSettings();
            var source = new List<Type>();
            foreach (string str in settings.EntityMapAssmbly)
            {
                var collection = from type in Assembly.Load(str).GetTypes()
                                 where !string.IsNullOrEmpty(type.Namespace)
                                 where ((type.BaseType != null) && type.BaseType.IsGenericType) && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
                                 select type;
                source.AddRange(collection);
            }
            foreach (Type type in source.Distinct<Type>())
            {
                var typeInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add((dynamic)typeInstance);
            }
            //所有的 decimal 类型格式默认为 18,3
            //modelBuilder.Properties<decimal>().Configure(e => e.HasPrecision(18, 3));

            base.OnModelCreating(modelBuilder);
        }
    }
}
