using Microsoft.EntityFrameworkCore;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupUser>()
                .HasKey(groupUser => new { groupUser.GroupId, groupUser.UserId });
            modelBuilder.Entity<GroupUser>()
                .HasOne(groupUser => groupUser.Group)
                .WithMany(@group => @group.GroupUsers)
                .HasForeignKey(groupUser => groupUser.GroupId);
            modelBuilder.Entity<GroupUser>()
                .HasOne(groupUser => groupUser.User)
                .WithMany(user => user.GroupUsers)
                .HasForeignKey(groupUser => groupUser.UserId);
            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(e => e.UserGuid).IsUnique();
                entity.HasIndex(e => e.Login).IsUnique();
            });
            modelBuilder.Entity<Group>(entity => {
                entity.HasIndex(e => e.GroupGuid).IsUnique();
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<User>()
                .HasOne(u => u.GroupInCall)
                .WithMany();
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Creator)
                .WithMany();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
