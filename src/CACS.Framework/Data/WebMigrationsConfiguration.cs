using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Data
{
    public class WebMigrationsConfiguration : DbMigrationsConfiguration<CACSWebObjectContext>
    {
        public WebMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CACSWebObjectContext context)
        {

        }
    }
}
