namespace Infrastructure.Auth.Interface
{
    /// <summary>
    /// 权限查询接口
    /// </summary>
    public interface IAuthQuery
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功，返回true</returns>
        bool Login(string userName, string password);

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="permissionName">权限名称</param>
        /// <returns>有权限，返回true</returns>
        bool HasRight(string permissionName);

    }
}