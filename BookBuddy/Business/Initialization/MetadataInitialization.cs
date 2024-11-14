using BookBuddy.Business.Extenders;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace BookBuddy.Business.Initialization;

[InitializableModule]
[ModuleDependency(typeof(CmsCoreInitialization))]
public class MetadataInitialization : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        if (context.HostType == HostType.WebApplication)
        {
            var registry = context.Locate.Advanced.GetInstance<MetadataHandlerRegistry>();
            registry.RegisterMetadataHandler(typeof(ContentData), new MetadataExtender());
        }
    }
    public void Uninitialize(InitializationEngine context)
    {
    }
}
