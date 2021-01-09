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
    }
}
