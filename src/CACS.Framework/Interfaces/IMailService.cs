using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Interfaces
{
    public interface IMailService
    {
        void SendMailByAddress(string[] addresses, string title, string message, bool isHtml);

        void SendMail(string[] users, string title, string message, bool isHtml);
    }
}
