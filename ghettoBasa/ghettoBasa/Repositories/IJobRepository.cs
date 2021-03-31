using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Repositories
{
    public interface IJobRepository
    {
        // Jobs Management
        IEnumerable<Jobs> GetJobs(string Token);
        IEnumerable<Jobs> GetUserJobs(string UserId, string Token);
        MyResponse GetPagiatedJobs(int page, int size, string Token);
        MyResponse GetPaginatedUserJobs(string UserId, int page, int size, string Token);
        Jobs GetJob(string JobId, string Token);
        void CreateJob(Jobs job, string Token);
        bool UpdateJob(Jobs job, string Token);
        bool JobDeleteStatus(string JobId, bool action, string Token);
        bool JobStatus(string JobId, string action, string Token);
        bool ReOpenJob(string JobId, string Token);
        IEnumerable<Jobs> GetDeletedJobs(string Token);
        IEnumerable<Jobs> SearchJobs(string[] Parameter);
        IEnumerable<Jobs> GetFilteredJobs(string[] Filters);


        // Job Bids Management
        IEnumerable<JobBids> GetJobBids(string JobId, string Token);
        IEnumerable<JobBids> GetUserJobBids(string UserId, string Token);
        IEnumerable<Jobs> GetUserSuccessfulJobBids(string UserId, string Token);
        JobBids GetJobBid(int JobId, string Token);
        void CreateJobBid(JobBids bids, string Token);
        bool JobBidDeleteStatus(int BidId, bool action, string Token);
        bool JobBidStatus(string JobId, string action, string Token);
        bool UpdateJobBid(JobBids jobBid, string Token);
        bool UpdateSuccessfulBidder(string JobId, string UserId, string Token);
    }
}
