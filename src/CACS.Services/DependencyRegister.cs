using CACS.Framework.Interfaces;
using CACS.Services;
using CACSLibrary.Infrastructure;

namespace CACS.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public EngineLevels Level
        {
            get { return EngineLevels.Priority; }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterComponent<IAccountService, AccountService>(typeof(AccountService).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent<ISystemService, SystemService>(typeof(SystemService).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent<IInstallation, Installation>(typeof(Installation).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent<ILogService, LogService>(typeof(LogService).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent<IMailService, MailService>(typeof(MailService).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent<IMessageService, MessageService>(typeof(MessageService).FullName, ComponentLifeStyle.LifetimeScope);
        }
    }
}
