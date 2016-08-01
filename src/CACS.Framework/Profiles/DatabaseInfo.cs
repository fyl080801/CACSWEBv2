using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Profiles
{
    public class DatabaseInfo : BaseProfile
    {
        public string DataProvider { get; set; }
        
        public string ConnectionString { get; set; }

        public List<string> EntityMapAssmbly { get; set; }

        public override ProfileObject GetDefault()
        {
            return new DatabaseInfo()
            {
                EntityMapAssmbly = new List<string>(),
                DataProvider = "sqlserver",
                ConnectionString = ""
            };
        }
    }
}
