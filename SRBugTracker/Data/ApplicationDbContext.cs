using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SRBugTracker.Models;

namespace SRBugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var entities = ChangeTracker.Entries().Where(e => (e.Entity is Project || e.Entity is Issue || e.Entity is Comment) && (e.State == EntityState.Added || e.State == EntityState.Modified));
                var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = Users.Find(userid);

                foreach (var entity in entities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        entity.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                        entity.Reference("CreatedBy").CurrentValue = user;
                    }
                    entity.Property("LastModifiedDate").CurrentValue = DateTime.UtcNow;
                    entity.Reference("LastModifiedBy").CurrentValue = user;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Issue>().Property(issue => issue.Status).HasConversion(v => v.ToString(), v => (Status)Enum.Parse(typeof(Status), v));

            builder.Entity<UserProject>().HasKey(userproject => new { userproject.UserId, userproject.ProjectId });
            builder.Entity<UserProject>()
                .HasOne(userproject => userproject.User)
                .WithMany(user => user.UserProjects)
                .HasForeignKey(userproject => userproject.UserId);
            builder.Entity<UserProject>()
                .HasOne(userproject => userproject.Project)
                .WithMany(project => project.UserProjects)
                .HasForeignKey(userproject => userproject.ProjectId);
        }

        public DbSet<SRBugTracker.Models.Issue> Issue { get; set; }
        public DbSet<SRBugTracker.Models.Comment> Comment { get; set; }
        public DbSet<SRBugTracker.Models.Project> Project { get; set; }
        public DbSet<SRBugTracker.Models.UserProject> UserProject { get; set; }
        public DbSet<SRBugTracker.Models.Role> Role { get; set; }
    }
}
