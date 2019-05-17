using System.Collections.Generic;

namespace Infrastructure.Auth.Shared
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<MappingRolePermission> MappingRolePermissions { get; set; } = new List<MappingRolePermission>();

        public virtual ICollection<MappingGroupPermission> MappingGroupPermissions { get; set; } = new List<MappingGroupPermission>();
    }
}
