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
            int? receiverId,
            DateTime? timeFrom,
            DateTime? timeTo,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> order);

        int GetNewMessageCount();

        int GetUserNewMessageCount(int userId);


        Message GetMessageById(int messageId);

        Message ReadMessageById(int messageId);

        void SendMessage(string title, string content, int typeId, int sender, int[] receivers);

        void DeleteMessages(int[] messages);
    }
}
