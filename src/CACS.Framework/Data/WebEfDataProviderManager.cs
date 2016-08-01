using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Data
{
	public class WebEfDataProviderManager : BaseDataProviderManager
	{
		public WebEfDataProviderManager(DatabaseSetting config)
			: base(config)
		{
		}

		public override IDataProvider LoadDataProvider()
		{
			string providerName = base.Setting.DataProvider;
			IDataProvider result;
			if (!string.IsNullOrWhiteSpace(providerName))
			{
				string text = providerName.ToLowerInvariant();
				if (text != null)
				{
					if (text == "sqlserver")
					{
						result = new WebSqlServerDataProvider();
						return result;
					}
				}
				throw new Exception(string.Format("不支持的 dataprovider: {0}", providerName));
			}
			result = new WebSqlServerDataProvider();
			return result;
		}
	}
}
