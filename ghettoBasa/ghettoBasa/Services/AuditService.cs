using ghettoBasa.Repositories;
using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Services
{
    public class AuditService : IAuditRepository
    {
        readonly ghettoBasaContext ctx;

        public AuditService(ghettoBasaContext context)
        {
            ctx = context;
        }

        public IEnumerable<AuditTrails> GetAuditTrails()
        {
            var trails = from t in ctx.AuditTrail
                         select t;

            return trails;
        }

        public MyResponse GetPaginatedTrails(int page, int size)
        {
            var trails = from t in ctx.AuditTrail
                       .OrderBy(cd => cd.Date)
                       .Skip(page * size)
                       .Take(size)
                       select t;

            var count = ctx.AuditTrail.Count();

            var response = new MyResponse()
            {
                totalElements = count,
                content = trails,
                totalPages = (int)(count / size),
                currentPage = page + 1
            };

            return response;
        }

        public AuditTrails GetTrail(int Id)
        {
            var trail = ctx.AuditTrail.FirstOrDefault(cd => cd.Id == Id);

            return trail;
        }
    }
}
