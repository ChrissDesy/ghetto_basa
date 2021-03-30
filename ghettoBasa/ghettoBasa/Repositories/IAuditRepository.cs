using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Repositories
{
    public interface IAuditRepository
    {
        IEnumerable<AuditTrails> GetAuditTrails();
        MyResponse GetPaginatedTrails(int page, int size);
        AuditTrails GetTrail(int Id);
    }
}
