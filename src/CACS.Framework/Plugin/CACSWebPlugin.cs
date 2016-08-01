using CACSLibrary.Web;
using CACSLibrary.Web.Plugin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml;

namespace CACS.Framework.Plugin
{
    public abstract class CACSWebPlugin : WebPlugin
    {
        protected void AddWcfService(string contractName, string serviceName, string relativeAddress, string binding)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            ServiceModelSectionGroup serviceModel = (ServiceModelSectionGroup)config.GetSectionGroup("system.serviceModel");
            RemoveExistService(serviceModel, serviceName);

            serviceModel.ServiceHostingEnvironment.MultipleSiteBindingsEnabled = true;
            serviceModel.ServiceHostingEnvironment.ServiceActivations.Add(new ServiceActivationElement(relativeAddress, serviceName));

            ServiceElement service = new ServiceElement(serviceName);
            service.BehaviorConfiguration = "serviceBehavior";
            service.Endpoints.Add(new ServiceEndpointElement()
            {
                Contract = contractName,
                BehaviorConfiguration = "endpointBehavior",
                Binding = binding
            });
            serviceModel.Services.Services.Add(service);

            config.Save();
        }

        protected void AddWcfService(string contractName, string serviceName, string relativeAddress)
        {
            AddWcfService(contractName, serviceName, relativeAddress, "basicHttpBinding");
        }

        protected void RemoveWcfService(string serviceName)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            ServiceModelSectionGroup serviceModel = (ServiceModelSectionGroup)config.GetSectionGroup("system.serviceModel");
            RemoveExistService(serviceModel, serviceName);
            config.Save();
        }

        private void RemoveExistService(ServiceModelSectionGroup serviceModel, string serviceName)
        {
            var activations = serviceModel.ServiceHostingEnvironment.ServiceActivations.GetEnumerator();
            while (activations.MoveNext())
            {
                var element = (ServiceActivationElement)activations.Current;
                if (element.Service == serviceName)
                {
                    serviceModel.ServiceHostingEnvironment.ServiceActivations.Remove(element);
                    break;
                }
            }
            var services = serviceModel.Services.Services.GetEnumerator();
            while (services.MoveNext())
            {
                var service = (ServiceElement)services.Current;
                if (service.Name == serviceName)
                {
                    serviceModel.Services.Services.Remove(service);
                    break;
                }
            }
        }
    }
}
