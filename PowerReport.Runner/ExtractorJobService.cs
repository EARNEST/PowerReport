using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;

namespace PowerReport.Runner
{
    public class ExtractorJobService
    {
        private ExtractorJob job;
        private BackgroundJobServer server;
        public ExtractorJobService()
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();
        }

        public async Task StartAsync()
        {
            this.job = new ExtractorJob();

            await this.job.StartAsync();

            this.server = new BackgroundJobServer();

            RecurringJob.AddOrUpdate(this.job.JobId, () => this.job.StartAsync(), this.job.Cron);
        }

        public void Stop()
        {
            this.job.Stop();

            RecurringJob.RemoveIfExists(this.job.JobId);

            this.server.SendStop();
        }
    }
}
