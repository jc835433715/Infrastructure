namespace Infrastructure.Auth.Shared
{
    public class MappingRoleGroup
    {
        public int RoleId { get; set; }

        public int GroupId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Group Group { get; set; }
    }
}
