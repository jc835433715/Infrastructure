namespace Infrastructure.Mvp.Interface
{
    /// <summary>
    /// Presenter接口
    /// </summary>
    public interface IPresenter
    {

    }

    /// <summary>
    /// Presenter接口
    /// </summary>
    /// <typeparam name="TView">View类型</typeparam>
    public interface IPresenter<out TView> : IPresenter
        where TView : class, IView
    {
        /// <summary>
        ///View
        /// </summary>
        TView View { get; }
    }
}