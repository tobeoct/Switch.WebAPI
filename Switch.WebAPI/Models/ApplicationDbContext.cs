using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Switch.WebAPI.Models
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<SinkNode> SinkNodes { get; set; }
        public DbSet<SourceNode> SourceNodes { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public ApplicationDbContext()
        {
            
        }
    }
}