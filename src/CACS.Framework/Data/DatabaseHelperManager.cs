using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CACS.Framework.Data
{
    public class DatabaseHelperManager
    {
        static Dictionary<string, IDatabaseHelper> _helpers = null;

        static DatabaseHelperManager()
        {
            IDatabaseHelper sqlHelper = new SqlServerDatabaseHelper();
            _helpers.Add(sqlHelper.ProviderName, sqlHelper);
        }

        public static IDatabaseHelper GetDatabaseHelper(string providerName)
        {
            if (_helpers.ContainsKey(providerName))
            {
                return _helpers[providerName];
            }
            else
            {
                throw new Exception(string.Format("不支持 {0}", providerName));
            }
        }
    }
}
