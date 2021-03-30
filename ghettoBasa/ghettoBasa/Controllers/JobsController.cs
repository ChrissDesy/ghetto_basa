using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ghettoBasa.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedResources.DTOs;
using SharedResources.Models;

namespace ghettoBasa.Controllers
{
    [Authorize]
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
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetJobs(tok);
        }

        [HttpGet("/api/[controller]/{page}/{size}")]
        public MyResponse GetPaginated(int page, int size)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetPagiatedJobs(page, size, tok);
        }

        [HttpGet("/api/[controller]/deleted")]
        public IEnumerable<Jobs> GetDeletedJobs()
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetDeletedJobs(tok);
        }

        [HttpGet("/api/[controller]/user/{userId}")]
        public IEnumerable<Jobs> GetUserJobs(string userId)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetUserJobs(userId, tok);
        }

        [HttpGet("/api/[controller]/user/{userId}/{page}/{size}")]
        public MyResponse GetPaginatedUserJobs(string userId, int page, int size)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetPaginatedUserJobs(userId, page, size, tok);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public Jobs GetJob(string id)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetJob(id, tok);
        }

        // POST: api/Jobs
        [HttpPost]
        public IActionResult Post(Jobs job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            _jobs.CreateJob(job, tok);

            return CreatedAtAction("GetJob", new { id = job.JobId }, job);
        }

        [HttpPost("/api/[controller]/renew-job/{jobId}")]
        public IActionResult RenewJob(string jobId)
        {
            if (jobId == null)
            {
                return BadRequest();
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.ReOpenJob(jobId, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobStatus(jobId, "active", tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobStatus(jobId, "disabled", tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.UpdateJob(job, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobDeleteStatus(jobId, false, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobDeleteStatus(jobId, true, tok);

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
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetJobBids(jobId, tok);
        }

        [HttpGet("/api/[controller]/bids/{Id}")]
        public JobBids GetJobBid(int Id)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetJobBid(Id, tok);
        }

        [HttpGet("/api/[controller]/bids/user/{userId}")]
        public IEnumerable<JobBids> GetUserJobBids(string userId)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetUserJobBids(userId, tok);
        }

        [HttpGet("/api/[controller]/successful-bids/user/{userId}")]
        public IEnumerable<Jobs> GetUserSuccessBids(string userId)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _jobs.GetUserSuccessfulJobBids(userId, tok);
        }

        [HttpPost("/api/[controller]/bids")]
        public IActionResult PostBid(JobBids bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            _jobs.CreateJobBid(bid, tok);

            return CreatedAtAction("GetJobBid", new { id = bid.Id }, bid);
        }

        [HttpPost("/api/[controller]/bids/success/{job}/{user}")]
        public IActionResult SuccessBid(string job, string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            _jobs.UpdateSuccessfulBidder(job, user, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.UpdateJobBid(bid, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobBidStatus(jobId, "disabled", tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobBidDeleteStatus(Id, false, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _jobs.JobBidDeleteStatus(Id, true, tok);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetJobBid(Id));
        }
    }
}
