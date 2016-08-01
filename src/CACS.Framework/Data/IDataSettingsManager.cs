using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CACS.Framework.Data
{
    public interface IDataSettingsManager
    {
        DatabaseSetting LoadSettings();

        void SaveSettings(DatabaseSetting setting);

        void ClearSettings();
    }
}
