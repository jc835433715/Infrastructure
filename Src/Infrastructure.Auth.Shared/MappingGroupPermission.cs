namespace Infrastructure.Auth.Shared
{
    public class MappingGroupPermission
    {
        public int GroupId { get; set; }

        public int PermissionId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
