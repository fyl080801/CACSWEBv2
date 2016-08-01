using CACS.Framework.Domain;
using CACS.Framework.Interfaces;
using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;
using System;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class LoggingAttribute : FilterAttribute, IActionFilter, IResultFilter
    {
        string _eventName;
        string _systemId;
        EventTypes _eventType;
        string _eventData;

        public EventTypes EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        public virtual string EventData
        {
            get { return _eventData; }
            set { _eventData = value; }
        }

        public LoggingAttribute(string eventName)
        {
            _eventName = eventName;
        }

        public LoggingAttribute(string eventName, string systemId)
            : this(eventName)
        {
            _systemId = systemId;
        }

        public virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid)
                ExecuteLog(filterContext);
        }

        public virtual void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

        protected virtual void ExecuteLog(ControllerContext context)
        {
            if (context.Controller.ViewData.ModelState.IsValid)
            {
                var log = EngineContext.Current.Resolve<ILogService>();
                log.AddLog(BuildLog());
            }
        }

        protected virtual EventLog BuildLog()
        {
            string sourceId = "";
            string sourcename = "";
            if (string.IsNullOrEmpty(_systemId))
            {
                sourceId = "系统";
                sourcename = "系统";
            }
            else
            {
                var pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
                var pluginDescription = pluginFinder.GetPluginDescriptorById(_systemId);
                if (pluginDescription != null && !string.IsNullOrEmpty(pluginDescription.PluginName))
                {
                    sourcename = pluginDescription.PluginName;
                }
                else
                {
                    sourcename = _systemId;
                }
            }
            EventLog log = new EventLog()
            {
                EventData = EventData,
                EventName = _eventName,
                EventType = EventType,
                SourceId = sourceId,
                SourceName = sourcename
            };
            return log;
        }
    }
}
