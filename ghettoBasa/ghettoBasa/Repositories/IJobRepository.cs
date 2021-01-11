using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Repositories
{
    public interface IJobRepository
    {
        IEnumerable<Jobs> GetJobs();
        IEnumerable<Jobs> GetUserJobs(string UserId);
        Jobs GetJob(string JobId);
        void CreateJob(Jobs job);
        bool UpdateJob(Jobs job);
        bool JobDeleteStatus(string JobId, bool action);
        bool JobStatus(string JobId, string action);
        bool ReOpenJob(string JobId);
        IEnumerable<Jobs> GetDeletedJobs();
        IEnumerable<Jobs> SearchJobs(string[] Parameter);
        IEnumerable<Jobs> GetFilteredJobs(string[] Filters);
    }
}
