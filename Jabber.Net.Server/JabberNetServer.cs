using System.Configuration;
using Jabber.Net.Server.Connections;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Jabber.Net.Server
{
    public class JabberNetServer : IXmppResolver
    {
        private IUnityContainer unityContainer;
        private XmppListenerManager listeners;


        public void Configure(string file)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = file };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
            unityContainer = new UnityContainer().LoadConfiguration(unitySection, "Jabber");

            unityContainer.RegisterInstance<IXmppResolver>(this, new ContainerControlledLifetimeManager());
            listeners = unityContainer.Resolve<XmppListenerManager>();
        }


        public void Start()
        {
            listeners.StartListen();
        }

        public void Stop()
        {
            listeners.StopListen();
        }

        public T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }
    }
}
