using ghettoBasa.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedResources.Models;
using System;
using System.Collections.Generic;
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

        public void CreateJob(Jobs job)
        {
            job.JobId = GenerateJobId();

            try
            {
                ctx.Jobs.Add(job);
                ctx.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        public IEnumerable<Jobs> GetDeletedJobs()
        {
            var jobs = from j in ctx.Jobs.Where(ab => ab.Deleted)
                       select j;

            return jobs;
        }

        public IEnumerable<Jobs> GetFilteredJobs(string[] Filters)
        {
            throw new NotImplementedException();
        }

        public Jobs GetJob(string JobId)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            return job;
        }

        public IEnumerable<Jobs> GetJobs()
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted)
                       select j;

            return jobs;
        }

        public IEnumerable<Jobs> GetUserJobs(string UserId)
        {
            var jobs = from j in ctx.Jobs.Where(ab => !ab.Deleted && ab.PosterId == UserId)
                       select j;

            return jobs;
        }

        public bool JobDeleteStatus(string JobId, bool action)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            job.Deleted = action;

            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool JobStatus(string JobId, string action)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            job.Status = action;

            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ReOpenJob(string JobId)
        {
            var job = ctx.Jobs.Where(ab => ab.JobId == JobId).FirstOrDefault();

            job.Renewed = "true";

            try
            {
                ctx.SaveChanges();
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

        public bool UpdateJob(Jobs job)
        {
            try
            {
                ctx.Entry(job).State = EntityState.Modified;

                ctx.SaveChanges();
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
    }
}
