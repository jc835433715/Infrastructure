using Infrastructure.Auth.Interface;
using System.Linq;

namespace Infrastructure.Auth.Imp
{
    public class AuthImp : IAuthRepository
    {
        static AuthImp()
        {
            userName = string.Empty;
        }

        public bool Login(string userName, string password)
        {
            var result = false;

            using (var context = new AuthDbContext())
            {
                result = context.Users.Any(e => e.Name == userName && e.Password == password);
            }

            if (result) AuthImp.userName = userName;

            return result;
        }

        public bool HasRight(string permissionName)
        {
            using (var context = new AuthDbContext())
            {
                return context.Users.Any(u => u.Name == userName && u.MappingUserRoles.Any(ur => ur.Role.MappingRolePermissions.Any(rp => rp.Permission.Name == permissionName)))
                    || context.Users.Any(u => u.Name == userName && u.MappingUserRoles.Any(ur => ur.Role.MappingRoleGroups.Any(rg => rg.Group.MappingGroupPermissions.Any(gp => gp.Permission.Name == permissionName))));
            }
        }

        private static string userName;
    }
}
