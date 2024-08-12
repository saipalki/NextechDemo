using CouncilVoting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CouncilVoting.Infrastructure.Persistance
{
    public class CouncilVotingContext : DbContext
    {
        public CouncilVotingContext(DbContextOptions<CouncilVotingContext> options) : base(options)
        {
        }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Voting> Votings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Scans a given assembly for all types that implement IEntityTypeConfiguration, and registers each one automatically
            //Mapping of Domain vs DBset entites
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
