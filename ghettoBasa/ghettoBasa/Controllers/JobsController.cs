using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ghettoBasa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
