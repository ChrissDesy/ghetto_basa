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
        IEnumerable<Jobs> GetJobs();
        IEnumerable<Jobs> GetUserJobs(string UserId);
        MyResponse GetPagiatedJobs(int page, int size);
        MyResponse GetPaginatedUserJobs(string UserId, int page, int size);
        Jobs GetJob(string JobId);
        void CreateJob(Jobs job);
        bool UpdateJob(Jobs job);
        bool JobDeleteStatus(string JobId, bool action);
        bool JobStatus(string JobId, string action);
        bool ReOpenJob(string JobId);
        IEnumerable<Jobs> GetDeletedJobs();
        IEnumerable<Jobs> SearchJobs(string[] Parameter);
        IEnumerable<Jobs> GetFilteredJobs(string[] Filters);


        // Job Bids Management
        IEnumerable<JobBids> GetJobBids(string JobId);
        IEnumerable<JobBids> GetUserJobBids(string UserId);
        IEnumerable<Jobs> GetUserSuccessfulJobBids(string UserId);
        JobBids GetJobBid(int JobId);
        void CreateJobBid(JobBids bids);
        bool JobBidDeleteStatus(int BidId, bool action);
        bool JobBidStatus(string JobId, string action);
        bool UpdateJobBid(JobBids jobBid);
        bool UpdateSuccessfulBidder(string JobId, string UserId);
    }
}
