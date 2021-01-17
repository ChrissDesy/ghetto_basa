using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ghettoBasa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedResources.DTOs;
using SharedResources.Models;

namespace ghettoBasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private IJobRepository _jobs;

        public JobsController(IJobRepository jobsRepository)
        {
            _jobs = jobsRepository;
        }

        // GET: api/Jobs
        [HttpGet]
        public IEnumerable<Jobs> Get()
        {
            return _jobs.GetJobs();
        }

        [HttpGet("/api/[controller]/{page}/{size}")]
        public MyResponse GetPaginated(int page, int size)
        {
            return _jobs.GetPagiatedJobs(page, size);
        }

        [HttpGet("/api/[controller]/deleted")]
        public IEnumerable<Jobs> GetDeletedJobs()
        {
            return _jobs.GetDeletedJobs();
        }

        [HttpGet("/api/[controller]/user/{userId}")]
        public IEnumerable<Jobs> GetUserJobs(string userId)
        {
            return _jobs.GetUserJobs(userId);
        }

        [HttpGet("/api/[controller]/user/{userId}/{page}/{size}")]
        public MyResponse GetPaginatedUserJobs(string userId, int page, int size)
        {
            return _jobs.GetPaginatedUserJobs(userId, page, size);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public Jobs GetJob(string id)
        {
            return _jobs.GetJob(id);
        }

        // POST: api/Jobs
        [HttpPost]
        public IActionResult Post(Jobs job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _jobs.CreateJob(job);

            return CreatedAtAction("GetJob", new { id = job.JobId }, job);
        }

        [HttpPost("/api/[controller]/renew-job/{jobId}")]
        public IActionResult RenewJob(string jobId)
        {
            if (jobId == null)
            {
                return BadRequest();
            }

            var resp = _jobs.ReOpenJob(jobId);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJob(jobId));
        }

        [HttpPost("/api/[controller]/enable-job/{jobId}")]
        public IActionResult EnableJob(string jobId)
        {
            if (jobId == null)
            {
                return BadRequest();
            }

            var resp = _jobs.JobStatus(jobId, "active");

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJob(jobId));
        }

        [HttpPost("/api/[controller]/disable-job/{jobId}")]
        public IActionResult DisableJob(string jobId)
        {
            if (jobId == null)
            {
                return BadRequest();
            }

            var resp = _jobs.JobStatus(jobId, "disabled");

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJob(jobId));
        }

        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, Jobs job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.JobId)
            {
                return BadRequest();
            }

            var resp = _jobs.UpdateJob(job);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(job);
        }

        // DELETE: api/ApiWithActions/5
        [HttpPost("/api/[controller]/un-delete/{jobId}")]
        public IActionResult UnDelete(string jobId)
        {
            if(jobId == null)
            {
                return BadRequest();
            }

            var resp = _jobs.JobDeleteStatus(jobId, false);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJob(jobId));
        }

        [HttpDelete("{jobId}")]
        public IActionResult Delete(string jobId)
        {
            if (jobId == null)
            {
                return BadRequest();
            }

            var resp = _jobs.JobDeleteStatus(jobId, true);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJob(jobId));
        }


        // Job Bids management
        [HttpGet("/api/[controller]/bids/job/{jobId}")]
        public IEnumerable<JobBids> GetJobBids(string jobId)
        {
            return _jobs.GetJobBids(jobId);
        }

        [HttpGet("/api/[controller]/bids/{Id}")]
        public JobBids GetJobBid(int Id)
        {
            return _jobs.GetJobBid(Id);
        }

        [HttpGet("/api/[controller]/bids/user/{userId}")]
        public IEnumerable<JobBids> GetUserJobBids(string userId)
        {
            return _jobs.GetUserJobBids(userId);
        }

        [HttpGet("/api/[controller]/successful-bids/user/{userId}")]
        public IEnumerable<Jobs> GetUserSuccessBids(string userId)
        {
            return _jobs.GetUserSuccessfulJobBids(userId);
        }

        [HttpPost("/api/[controller]/bids")]
        public IActionResult PostBid(JobBids bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _jobs.CreateJobBid(bid);

            return CreatedAtAction("GetJobBid", new { id = bid.Id }, bid);
        }

        [HttpPost("/api/[controller]/bids/success/{job}/{user}")]
        public IActionResult SuccessBid(string job, string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _jobs.UpdateSuccessfulBidder(job, user);

            return Ok(GetJob(job));
        }

        [HttpPut("/api/[controller]/bids/{id}")]
        public IActionResult PutBid(int id, JobBids bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bid.Id)
            {
                return BadRequest();
            }

            var resp = _jobs.UpdateJobBid(bid);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(bid);
        }

        [HttpPut("/api/[controller]/bids/job-bids/{jobId}")]
        public IActionResult PutJobBids(string jobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (jobId == null)
            {
                return BadRequest();
            }

            var resp = _jobs.JobBidStatus(jobId, "disabled");

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJob(jobId));
        }

        [HttpPost("/api/[controller]/bids/un-delete/{Id}")]
        public IActionResult UnDeleteBid(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var resp = _jobs.JobBidDeleteStatus(Id, false);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJobBid(Id));
        }

        [HttpDelete("/api/[controller]/bids/delete/{Id}")]
        public IActionResult DeleteBid(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var resp = _jobs.JobBidDeleteStatus(Id, true);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJobBid(Id));
        }
    }
}
