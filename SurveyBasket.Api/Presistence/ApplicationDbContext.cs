

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace SurveyBasket.Api.Presistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ,
        IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser ,ApplicationRole , string>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteAnswer> VoteAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollConfigurations()); 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //changing delete behavior globally
            var cascadeForeignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).
                Where(t => t.DeleteBehavior == DeleteBehavior.Cascade && !t.IsOwnership);

            foreach (var foreignKey in cascadeForeignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //extract the user id using the interface _httpContextAccessor 
            var UserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            //track only entites which inherits from "AuditableEntity"
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var entry in entries)
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedById).CurrentValue = UserId;
                    //no need for this i already implemented it in class body
                    //entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                }
                else if(entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedById).CurrentValue = UserId;
                    entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
