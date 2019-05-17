using System.Collections.Generic;

namespace Infrastructure.Auth.Shared
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public  virtual  ICollection<MappingUserRole> MappingUserRoles { get; set; } = new List<MappingUserRole>();
    }
}
