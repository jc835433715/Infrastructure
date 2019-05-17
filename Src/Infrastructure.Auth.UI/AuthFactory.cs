using Infrastructure.Auth.Imp;
using Infrastructure.Auth.Interface;

namespace Infrastructure.Auth.UI
{
    static class AuthFactory
    {
        public static IAuthRepository CreateAuth()
        {
            return new AuthImp();
        }
    }
}
