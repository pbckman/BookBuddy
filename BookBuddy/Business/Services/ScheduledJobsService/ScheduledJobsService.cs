using System;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;

namespace BookBuddy.Business.Services.ScheduledJobsService;

public class ScheduledJobsService : IScheduledJobsService
{
    public void TriggerIndexing()
    {
        var jobName = "Search & Navigation Content Indexing Job";
        
        var jobRepository = ServiceLocator.Current.GetInstance<IScheduledJobRepository>();
        var jobExecutor = ServiceLocator.Current.GetInstance<IScheduledJobExecutor>();
        var indexingJob = jobRepository.List()
                               .FirstOrDefault(job => job.Name.Equals(jobName, StringComparison.OrdinalIgnoreCase));

        if (indexingJob != null)
        {
            jobExecutor.StartAsync(indexingJob);
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("ERROR : ScheduledJobsService.TriggerIndexing() - Indexing job not found");
        }

    }
}
