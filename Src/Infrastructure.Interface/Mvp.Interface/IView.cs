using System;

namespace Infrastructure.Mvp.Interface
{
    /// <summary>
    ///View接口
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Load事件
        /// </summary>
        event EventHandler Load;
    }

    /// <summary>
    /// View接口
    /// </summary>
    /// <typeparam name="TModel">Model类型</typeparam>
    public interface IView<TModel> : IView
    {
        /// <summary>
        /// Model
        /// </summary>
        TModel Model { get; set; }
    }
}
