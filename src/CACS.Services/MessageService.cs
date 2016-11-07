using System;
using System.Collections.Generic;
using System.Linq;
using CACS.Framework.Domain;
using CACS.Framework.Interfaces;
using CACSLibrary;
using CACSLibrary.Data;


namespace CACS.Services
{
    public class MessageService : IMessageService
    {
        IRepository<Message> _messageRepository;

        public MessageService(
            IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public IPagedList<Message> GetAllMessage(
            string title,
            bool? isReaded,
            int[] typeIds,
            int? receiverId,
            DateTime? timeFrom,
            DateTime? timeTo,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> order)
        {
            //var query = from c in _messageRepository.Table where c.ReceiverId == receiverId select c;
            var query = _messageRepository.Table;
            query = query.Where(c => c.ReceiverId == receiverId);
            if (timeFrom.HasValue)
                query = query.Where(c => timeFrom.Value <= c.SendTime);
            if (timeTo.HasValue)
                query = query.Where(c => timeTo.Value >= c.SendTime);
            if (isReaded.HasValue && isReaded.Value)
                query = query.Where(c => c.ReadTime.HasValue);
            else if (isReaded.HasValue && !isReaded.Value)
                query = query.Where(c => !c.ReadTime.HasValue);
            if (typeIds != null && typeIds.Length > 0)
                query = query.Where(m => typeIds.Contains((int)m.MessageType));
            if (!String.IsNullOrWhiteSpace(title))
                query = query.Where(c => c.Title.Contains(title));

            if (order != null && order.Count > 0)
            {
                foreach (var sortItem in order)
                {
                    if (sortItem.Key == "SenderName")
                        query = QueryBuilder.DataSorting(query, "SenderId", sortItem.Value);
                    else
                        query = QueryBuilder.DataSorting(query, sortItem.Key, sortItem.Value);
                }
            }
            else
            {
                query = query.OrderByDescending(m => m.SendTime);
            }
            var messages = new PagedList<Message>(query, pageIndex, pageSize);
            if (messages == null)
            {
                messages = new PagedList<Message>(new List<Message>(), pageIndex, pageSize);
            }
            return messages;
        }

        //public IPagedList<Message> GetAllSendedMessage(
        //    string title,
        //    bool? isReaded,
        //    int[] typeIds,
        //    int senderId,
        //    DateTime? timeFrom,
        //    DateTime? timeTo,
        //    int pageIndex,
        //    int pageSize,
        //    IDictionary<string, bool> order)
        //{
        //    var query = from c in _messageRepository.Table where c.SenderId == senderId select c;
        //    if (timeFrom.HasValue)
        //        query = query.Where(c => timeFrom.Value <= c.SendTime);
        //    if (timeTo.HasValue)
        //        query = query.Where(c => timeTo.Value >= c.SendTime);
        //    if (isReaded.HasValue && isReaded.Value)
        //        query = query.Where(c => c.ReadTime.HasValue);
        //    else if (isReaded.HasValue && !isReaded.Value)
        //        query = query.Where(c => !c.ReadTime.HasValue);
        //    if (typeIds != null && typeIds.Length > 0)
        //        query = query.Where(m => typeIds.Contains((int)m.MessageType));
        //    if (!String.IsNullOrWhiteSpace(title))
        //        query = query.Where(c => c.Title.Contains(title));

        //    if (order != null && order.Count > 0)
        //    {
        //        foreach (var sortItem in order)
        //        {
        //            query = QueryBuilder.DataSorting(query, sortItem.Key, sortItem.Value);
        //        }
        //    }
        //    else
        //    {
        //        query = query.OrderByDescending(m => m.SendTime);
        //    }
        //    var messages = new PagedList<Message>(query, pageIndex, pageSize);
        //    if (messages == null)
        //    {
        //        messages = new PagedList<Message>(new List<Message>(), pageIndex, pageSize);
        //    }
        //    return messages;
        //}

        public int GetUserNewMessageCount(int userId)
        {
            return _messageRepository.Table.Count(m => m.ReceiverId == userId && m.ReadTime == null);
        }

        public int GetNewMessageCount()
        {
            return _messageRepository.Table.Count(m => m.ReadTime == null);
        }

        public Message GetMessageById(int messageId)
        {
            return _messageRepository.GetById(messageId);
        }

        public Message ReadMessageById(int messageId)
        {
            var message = _messageRepository.GetById(messageId);
            message.ReadTime = DateTime.Now;
            _messageRepository.Update(message);
            return message;
        }

        public void SendMessage(string title, string content, int typeId, int sender, int[] receivers)
        {
            List<Message> messages = new List<Message>();
            foreach (int receiver in receivers)
            {
                Message message = new Message();
                message.Content = content;
                message.MessageType = (MessageTypes)typeId;
                message.ReceiverId = receiver;
                message.SenderId = sender;
                message.SendTime = DateTime.Now;
                message.Title = title;
                messages.Add(message);
            }
            _messageRepository.Insert(messages.ToArray());
        }

        public void DeleteMessages(int[] messages)
        {
            var query = (from c in _messageRepository.Table where messages.Contains(c.Id) select c).ToArray();
            _messageRepository.Delete(query);
        }
    }
}
