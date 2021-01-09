using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public partial class ghettoBasaContext : DbContext
    {
        public ghettoBasaContext()
        {

        }

        public ghettoBasaContext(DbContextOptions<ghettoBasaContext> options) : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<AuditTrails> AuditTrail { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<JobBids> JobBids { get; set; }
        public virtual DbSet<Ratings> Ratings { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<SystemUsers> SystemUsers { get; set; }
    }
}
