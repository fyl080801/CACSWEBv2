using CACS.Framework.Domain;
using CACSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Interfaces
{
    public interface IMessageService
    {
        IPagedList<Message> GetAllMessage(
            string title,
            bool? isReaded,
            int[] typeIds,
            string receiverId,
            DateTime? timeFrom,
            DateTime? timeTo,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> order);

        int GetNewMessageCount();

        int GetUserNewMessageCount(string userId);


        Message GetMessageById(int messageId);

        Message ReadMessageById(int messageId);

        void SendMessage(string title, string content, int typeId, string sender, string[] receivers);

        void DeleteMessages(int[] messages);
    }
}
