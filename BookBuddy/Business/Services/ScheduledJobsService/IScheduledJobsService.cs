using System;

namespace BookBuddy.Business.Services.ScheduledJobsService;

public interface IScheduledJobsService
{
    public void TriggerIndexing();
}
