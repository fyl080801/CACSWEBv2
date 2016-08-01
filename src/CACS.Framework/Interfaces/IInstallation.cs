using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Interfaces
{
    public interface IInstallation
    {
        void InstallData(string adminPassword, string adminMail, string systemMail);
    }
}
