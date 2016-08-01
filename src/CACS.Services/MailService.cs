using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Profiles;
using CACSLibrary;
using CACSLibrary.Profile;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace CACS.Services
{
    public class MailService : IMailService
    {
        readonly ApplicationUserManager _userManager;
        readonly IProfileManager _profileManager;

        public MailService(
            IProfileManager profileManager,
            ApplicationUserManager userManager)
        {
            _profileManager = profileManager;
            _userManager = userManager;
        }

        public void SendMail(string[] users, string title, string message, bool isHtml)
        {
            var query = _userManager.Users.Where(c => users.Contains(c.Id)).Select(c => c.Email).ToArray();
            SendMailByAddress(query, title, message, isHtml);
        }

        public void SendMailByAddress(string[] addresses, string title, string message, bool isHtml)
        {
            SmtpSettings profile = _profileManager.Get<SmtpSettings>();
            if (string.IsNullOrEmpty(profile.Server))
                throw new CACSException("未设置邮件发送服务器地址");
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(profile.Address);
            mm.Subject = title;
            mm.Body = message;
            mm.IsBodyHtml = isHtml;

            foreach (string address in addresses)
            {
                mm.To.Add(new MailAddress(address));
            }
            SmtpClient smtp = new SmtpClient(profile.Server, profile.Port);
            if (profile.IsCredential)
            {
                smtp.Credentials = new NetworkCredential(profile.Username, profile.Password);
            }
            smtp.SendAsync(mm, null);
        }
    }
}
