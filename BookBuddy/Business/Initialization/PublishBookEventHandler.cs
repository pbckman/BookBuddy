using BookBuddy.Models.Pages;
using EPiServer.Framework.Initialization;
using EPiServer.Framework;
using EPiServer.Globalization;
using EPiServer.Scheduler;
using EPiServer.Web;
using EPiServer.ServiceLocation;

namespace BookBuddy.Business.Initialization;

[InitializableModule]
[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
public class PublishBookEventHandler : IInitializableModule
{
    private IContentEvents _contentEvents;
    private IScheduledJobExecutor _scheduledJobExecutor;
    private IContentRepository _contentRepository;
    private ISiteDefinitionRepository _siteDefinitionRepository;
    private IContentLoader _contentLoader;
    public static string CurrentCultureName { get; private set; }

    public void Initialize(InitializationEngine context)
    {
        _contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
        _scheduledJobExecutor = ServiceLocator.Current.GetInstance<IScheduledJobExecutor>();
        _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        _siteDefinitionRepository = ServiceLocator.Current.GetInstance<ISiteDefinitionRepository>();
        _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        _contentEvents.PublishedContent += OnPublishedContent;
    }

    private void OnPublishedContent(object sender, ContentEventArgs e)
    {
        if (e.Content is AvailableBooksPage)
        {

            CurrentCultureName = (e.Content as ILocalizable)?.Language.Name ?? ContentLanguage.PreferredCulture.Name;

            var jobRepository = ServiceLocator.Current.GetInstance<IScheduledJobRepository>();
            var job = jobRepository.Get(new Guid("C5F818B6-B864-4E5C-837B-49BCB98D14EE"));

            if (job != null)
            {
                _scheduledJobExecutor.StartAsync(job);
            }
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
        _contentEvents.PublishedContent -= OnPublishedContent;
    }
}
