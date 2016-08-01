using CACS.Framework.Domain;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;
using System.Linq;

namespace CACS.Framework.Profiles
{
    public class UserDatabaseProfileProvider : IProfileProvider
    {
        IRepository<Profile> _profileRepository;

        public UserDatabaseProfileProvider()
        {
            _profileRepository = EngineContext.Current.Resolve<IRepository<Profile>>();
        }

        public void Clear(object config)
        {
            //var cfg = _profileRepository.Table.FirstOrDefault(e => e.Key == config.GetType().FullName && e.UserId == _userContext.CurrentUser.Id);
            //if (cfg != null)
            //{
            //    _profileRepository.Delete(cfg);
            //}
        }

        public object Load(Type configType)
        {
            //            var cfg = _profileRepository.Table.FirstOrDefault(e => e.Key == configType.FullName && e.UserId == _userContext.CurrentUser.Id);
            //            if (cfg != null)
            //            {
            //                try
            //                {
            //                    var profileobj = JsonConvert.DeserializeObject(cfg.Value, configType, this.CreateSetting());
            //                    return profileobj;
            //                }
            //                catch (Exception ex)
            //                {
            //#if DEBUG
            //                    Debug.WriteLine(ex.Message);
            //#endif
            //                }
            //            }
            return null;
        }

        public void Save(object config)
        {
            string data = JsonConvert.SerializeObject(config, Formatting.Indented, this.CreateSetting());
            Profile profile = new Profile()
            {
                Key = config.GetType().FullName,
                //UserId = _userContext.CurrentUser.Id,
                Value = data
            };
            _profileRepository.Insert(profile);
        }

        private JsonSerializerSettings CreateSetting()
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(timeFormat);
            return setting;
        }
    }
}
