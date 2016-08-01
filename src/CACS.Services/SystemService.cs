using CACS.Framework.Interfaces;
using CACS.Framework.Profiles;
using CACSLibrary.Profile;

namespace CACS.Services
{
    public class SystemService : ISystemService
    {
        readonly IProfileManager _profileManager;

        public SystemService(IProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        public SystemInformation GetSystemInformation()
        {
            var info = _profileManager.Get<SystemInformation>();
            return info;
        }
    }
}
