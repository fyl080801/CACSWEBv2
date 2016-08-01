using CACS.Framework.Profiles;
using CACSLibrary.Data;
using CACSLibrary.Profile;

namespace CACS.Framework.Data
{
    public class DataSettingsManager : IDataSettingsManager
    {
        IProfileManager _profileManager;

        public DataSettingsManager(IProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        public DatabaseSetting LoadSettings()
        {
            DatabaseInfo config = _profileManager.Get<DatabaseInfo>();
            DatabaseSetting setting = new DatabaseSetting();
            setting.ConnectionString = config.ConnectionString;
            setting.DataProvider = config.DataProvider;
            setting.EntityMapAssmbly = config.EntityMapAssmbly;
            return setting;
        }

        public void SaveSettings(DatabaseSetting setting)
        {
            DatabaseInfo config = _profileManager.Get<DatabaseInfo>();
            config.ConnectionString = setting.ConnectionString;
            config.DataProvider = setting.DataProvider;
            config.EntityMapAssmbly = setting.EntityMapAssmbly;
            _profileManager.Save(config);
        }

        public void ClearSettings()
        {
            DatabaseInfo config = _profileManager.Get<DatabaseInfo>();
            _profileManager.Clear(config);
        }
    }
}
