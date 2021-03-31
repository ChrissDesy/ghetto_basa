using ghettoBasa.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Services
{
    public class JobsService : IJobRepository
    {
        private static Random random = new Random();
        readonly ghettoBasaContext ctx;

        public JobsService(ghettoBasaContext context)
        {
            ctx = context;
        }

        // Job Management
        public void CreateJob(Jobs job, string Token)
        {
            job.JobId = GenerateJobId();

            try
            {
                ctx.Jobs.Add(job);
                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Create Job",
                        Service = "Jobs Service",
                        Description = "Create a new job."
                    };
                    createTrail(trail);
                }
            }
            catch
            {
                return;
            }
        }

        public IEnumerable<Jobs> GetDeletedJobs(string Token)
        {
            var jobs = from j in ctx.Jobs.Where(ab => ab.Deleted)
                       select j;

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Jobs",
                    Service = "Jobs Service",
                    Description = "Get a list of deleted jobs."
                };
                createTrail(trail);
            }

            return jobs;
        }

        public MyResponse GetPagiatedJobs(int page, int size, string Token)
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted)
                       .OrderBy(cd => cd.DatePosted)
                       .Skip(page * size)
                       .Take(size)
                       select j;

            var count = ctx.Jobs.Where(ab => !ab.Deleted).Count();

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Jobs",
                    Service = "Jobs Service",
                    Description = "Get paginated list of jobs."
                };
                createTrail(trail);
            }

            var response = new MyResponse()
            {
                totalElements = count,
                content = jobs,
                totalPages = (int)(count / size),
                currentPage = page + 1
            };

            return response;
        }

        public MyResponse GetPaginatedUserJobs(string UserId, int page, int size, string Token)
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted && ab.PosterId == UserId)
                       .OrderBy(cd => cd.DatePosted)
                       .Skip(page * size)
                       .Take(size)
                       select j;

            var count = ctx.Jobs.Where(ab => !ab.Deleted && ab.PosterId == UserId).Count();

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Jobs",
                    Service = "Jobs Service",
                    Description = "Get paginated list of user jobs."
                };
                createTrail(trail);
            }

            var response = new MyResponse()
            {
                totalElements = count,
                content = jobs,
                totalPages = (int)(count / size),
                currentPage = page + 1
            };

            return response;
        }

        public IEnumerable<Jobs> GetFilteredJobs(string[] Filters)
        {
            throw new NotImplementedException();
        }

        public Jobs GetJob(string JobId, string Token)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Job",
                    Service = "Jobs Service",
                    Description = "Get job details."
                };
                createTrail(trail);
            }

            return job;
        }

        public IEnumerable<Jobs> GetJobs(string Token)
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted)
                       select j;

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Jobs",
                    Service = "Jobs Service",
                    Description = "Get a list of jobs."
                };
                createTrail(trail);
            }

            return jobs;
        }

        public IEnumerable<Jobs> GetUserJobs(string UserId, string Token)
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted && ab.PosterId == UserId)
                       select j;

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Jobs",
                    Service = "Jobs Service",
                    Description = "Get a list of user jobs."
                };
                createTrail(trail);
            }

            return jobs;
        }

        public bool JobDeleteStatus(string JobId, bool action, string Token)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            job.Deleted = action;

            try
            {
                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Delete Job",
                        Service = "Jobs Service",
                        Description = "Delete a job."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool JobStatus(string JobId, string action, string Token)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            job.Status = action;

            try
            {
                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Job status",
                        Service = "Jobs Service",
                        Description = "Change a job's status."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ReOpenJob(string JobId, string Token)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            job.Renewed = "true";

            try
            {
                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Reopen Job",
                        Service = "Jobs Service",
                        Description = "Reopen a job."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Jobs> SearchJobs(string[] Parameter)
        {
            throw new NotImplementedException();
        }

        public bool UpdateJob(Jobs job, string Token)
        {
            try
            {
                ctx.Entry(job).State = EntityState.Modified;

                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Update Job",
                        Service = "Jobs Service",
                        Description = "Update job details."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        string GenerateJobId()
        {
            var unum = DateTime.Now.Day.ToString() + RandomNumbers(2) + DateTime.Now.Month.ToString();

            var gid = unum + RandomString(1);

            return gid;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumbers(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }


        // Job Bids Management
        public IEnumerable<JobBids> GetJobBids(string JobId, string Token)
        {
            var bids = from j in ctx.JobBids.Where(ab => ab.JobRef == JobId && !ab.Deleted && ab.Status == "active")
                       select j;

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Bids",
                    Service = "Jobs Service",
                    Description = "Get a list of job bids."
                };
                createTrail(trail);
            }

            return bids;
        }

        public IEnumerable<JobBids> GetUserJobBids(string UserId, string Token)
        {
            var bids = from j in ctx.JobBids.Where(ab => ab.BidderId == UserId && !ab.Deleted)
                       select j;

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Bids",
                    Service = "Jobs Service",
                    Description = "Get a list of user job bids."
                };
                createTrail(trail);
            }

            return bids;
        }

        public JobBids GetJobBid(int Id, string Token)
        {
            var bid = ctx.JobBids.Where(ab => ab.Id == Id).FirstOrDefault();
            var obid = bid;

            if (bid != null && !bid.Viewed)
            {
                bid.Viewed = true;
                ctx.Entry(bid).State = EntityState.Modified;
            }

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Bid",
                    Service = "Jobs Service",
                    Description = "Get job bid details."
                };
                createTrail(trail);
            }

            return obid;
        }

        public void CreateJobBid(JobBids bid, string Token)
        {
            try
            {
                ctx.JobBids.Add(bid);
                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Create Bid",
                        Service = "Jobs Service",
                        Description = "Create a job bid."
                    };
                    createTrail(trail);
                }
            }
            catch
            {
                return;
            }
        }

        public bool JobBidDeleteStatus(int BidId, bool action, string Token)
        {
            var job = ctx.JobBids.Where(ab => ab.Id == BidId).FirstOrDefault();

            job.Deleted = action;

            try
            {
                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Delete Bid",
                        Service = "Jobs Service",
                        Description = "Delete a job bid."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool JobBidStatus(string JobId, string action, string Token)
        {
            var bids = ctx.JobBids.Where(ab => ab.Status == "active" && ab.JobRef == JobId && !ab.Deleted);

            try
            {
                foreach(var bid in bids)
                {
                    bid.Status = action;

                    ctx.Entry(bid).State = EntityState.Modified;
                }

                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Bid Status",
                        Service = "Jobs Service",
                        Description = "Change job bids status."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateJobBid(JobBids jobBid, string Token)
        {
            try
            {
                ctx.Entry(jobBid).State = EntityState.Modified;

                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Update Bid",
                        Service = "Jobs Service",
                        Description = "Update a job bid."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Jobs> GetUserSuccessfulJobBids(string UserId, string Token)
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted && ab.SuccessfulBidder == UserId)
                       select j;

            if (Token != null)
            {
                var rec = getUsername(Token);
                var trail = new AuditTrails()
                {
                    UserRefere = rec.Item2,
                    Username = rec.Item1,
                    Action = "Get Bids",
                    Service = "Jobs Service",
                    Description = "Get a list of user successful bids."
                };
                createTrail(trail);
            }

            return jobs;
        }

        public bool UpdateSuccessfulBidder(string JobId, string UserId, string Token)
        {
            var job = ctx.Jobs.FirstOrDefault(ab => ab.JobId == JobId);

            job.SuccessfulBidder = UserId;

            try
            {
                ctx.Entry(job).State = EntityState.Modified;

                ctx.SaveChanges();

                if (Token != null)
                {
                    var rec = getUsername(Token);
                    var trail = new AuditTrails()
                    {
                        UserRefere = rec.Item2,
                        Username = rec.Item1,
                        Action = "Update Bid",
                        Service = "Jobs Service",
                        Description = "Update successful bidder."
                    };
                    createTrail(trail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // get logged in user from token
        public Tuple<string, string> getUsername(string theToken)
        {
            var tok = "";

            tok = theToken.Substring(7);

            var token = new JwtSecurityToken(tok);
            var uname = token.Claims.First(cl => cl.Type == "name");
            var userId = token.Claims.First(cl => cl.Type == "act");

            return new Tuple<string, string>(uname.Value, userId.Value);
        }

        // post Trail
        public bool createTrail(AuditTrails trail)
        {
            try
            {
                ctx.AuditTrail.Add(trail);
                ctx.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
