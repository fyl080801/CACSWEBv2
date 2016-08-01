using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CACS.Framework.Data
{
    public class SqlServerDatabaseHelper : IDatabaseHelper
    {
        Dictionary<string, object> _variables;

        //返回
        //----------------------------------------------------------------------------
        // ServerName        |   服务器的名称
        //----------------------------------------------------------------------------
        // InstanceName    |   服务器实例的名称。如果服务器作为默认实例运行，则为空白
        //----------------------------------------------------------------------------
        // IsClustered         |   指示服务器是否属于群集
        //----------------------------------------------------------------------------
        // Version               |   服务器的版本(SQLServer2000为8.00.x,SQLServer2005为9.00.x)
        //----------------------------------------------------------------------------

        public string ProviderName
        {
            get { return "sqlserver"; }
        }

        public string CreateConnectionString()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.Password = this.Variables.ContainsKey("password") ? this.Variables["password"].ToString() : "";
            sb.UserID = this.Variables.ContainsKey("userid") ? this.Variables["userid"].ToString() : "";
            sb.PersistSecurityInfo = this.Variables.ContainsKey("persistSecurityInfo") ? Convert.ToBoolean(this.Variables.ContainsKey("persistSecurityInfo")) : true;
            sb.DataSource = this.Variables.ContainsKey("dataSource") ? this.Variables["dataSource"].ToString() : ".";
            sb.InitialCatalog = this.Variables.ContainsKey("initialCatalog") ? this.Variables["initialCatalog"].ToString() : "";
            return sb.ConnectionString;
        }

        public Dictionary<string, object> Variables
        {
            get { return _variables ?? (_variables = new Dictionary<string, object>()); }
        }
    }
}
