using System.Collections.Generic;

namespace Infrastructure.Auth.Shared
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public  virtual  ICollection<MappingRoleGroup> MappingRoleGroups { get; set; } = new List<MappingRoleGroup>();

        public  virtual  ICollection<MappingGroupPermission> MappingGroupPermissions { get; set; } = new List<MappingGroupPermission>();
    }
}
