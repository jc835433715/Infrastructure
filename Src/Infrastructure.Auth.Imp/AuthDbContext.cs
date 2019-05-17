using Infrastructure.Auth.Shared;
using System.Data.Entity;

namespace Infrastructure.Auth.Imp
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext() :
            base("name = AuthDbContext")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(User));

            modelBuilder.Entity<Role>().ToTable(nameof(Role));

            modelBuilder.Entity<Group>().ToTable(nameof(Group));

            modelBuilder.Entity<MappingUserRole>().ToTable(nameof(MappingUserRole)).HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<MappingUserRole>()
                .HasRequired(t => t.User)
                .WithMany(t => t.MappingUserRoles)
                .HasForeignKey(t => t.UserId);
            modelBuilder.Entity<MappingUserRole>()
                .HasRequired(t => t.Role)
                .WithMany(t => t.MappingUserRoles)
                .HasForeignKey(t => t.RoleId);
            
            modelBuilder.Entity<MappingRoleGroup>().ToTable(nameof(MappingRoleGroup)).HasKey(t => new { t.RoleId, t.GroupId });
            modelBuilder.Entity<MappingRoleGroup>()
                .HasRequired(t => t.Role)
                .WithMany(t => t.MappingRoleGroups)
                .HasForeignKey(t => t.RoleId);
            modelBuilder.Entity<MappingRoleGroup>()
                .HasRequired(t => t.Group)
                .WithMany(t => t.MappingRoleGroups)
                .HasForeignKey(t => t.GroupId);
            modelBuilder.Entity<MappingRolePermission>().ToTable(nameof(MappingRolePermission)).HasKey(t => new { t.RoleId, t.PermissionId });
            modelBuilder.Entity<MappingRolePermission>()
                .HasRequired(t => t.Role)
                .WithMany(t => t.MappingRolePermissions)
                .HasForeignKey(t => t.RoleId);
            modelBuilder.Entity<MappingRolePermission>()
                .HasRequired(t => t.Permission)
                .WithMany(t => t.MappingRolePermissions)
                .HasForeignKey(t => t.PermissionId);

            modelBuilder.Entity<MappingGroupPermission>().ToTable(nameof(MappingGroupPermission)).HasKey(t => new { t.GroupId, t.PermissionId });
            modelBuilder.Entity<MappingGroupPermission>()
                .HasRequired(t => t.Group)
                .WithMany(t => t.MappingGroupPermissions)
                .HasForeignKey(t => t.GroupId);
            modelBuilder.Entity<MappingGroupPermission>()
                .HasRequired(t => t.Permission)
                .WithMany(t => t.MappingGroupPermissions)
                .HasForeignKey(t => t.PermissionId);
        }
    }
}
