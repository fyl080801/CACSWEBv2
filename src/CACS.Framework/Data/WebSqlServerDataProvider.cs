using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;

namespace CACS.Framework.Data
{
    public class WebSqlServerDataProvider : SqlServerDataProvider
    {
        public override void SetDatabaseInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CACSWebObjectContext, WebMigrationsConfiguration>());
        }

        public override IDbConnectionFactory GetConnectionFactory()
        {
            var settings = EngineContext.Current.Resolve<IDataSettingsManager>().LoadSettings();
            return new SqlConnectionFactory(settings.ConnectionString);
        }
    }
}
