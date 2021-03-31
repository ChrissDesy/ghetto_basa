using ghettoBasa.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ghettoBasa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditTrailsController : ControllerBase
    {
        private IAuditRepository _audit; 

        public AuditTrailsController(IAuditRepository audit)
        {
            _audit = audit;
        }

        // GET: api/<AuditTrailsController>
        [HttpGet]
        public IEnumerable<AuditTrails> GetAll()
        {
            return _audit.GetAuditTrails();
        }

        // GET api/<AuditTrailsController>/0/10
        [HttpGet("{page}/{size}")]
        public MyResponse GetPaginated(int page, int size)
        {
            return _audit.GetPaginatedTrails(page, size);
        }

        // GET api/<AuditTrailsController>/5
        [HttpGet("{id}")]
        public AuditTrails GetTrail(int id)
        {
            return _audit.GetTrail(id);
        }
    }
}
