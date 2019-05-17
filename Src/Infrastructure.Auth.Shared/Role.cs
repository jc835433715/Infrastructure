using System.Collections.Generic;

namespace Infrastructure.Auth.Shared
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<MappingUserRole> MappingUserRoles { get; set; } = new List<MappingUserRole>();

        public virtual ICollection<MappingRoleGroup> MappingRoleGroups { get; set; } = new List<MappingRoleGroup>();

        public virtual ICollection<MappingRolePermission> MappingRolePermissions { get; set; } = new List<MappingRolePermission>();
    }
}
