using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 线程帮助者
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// 延时
        /// </summary>
        /// <param name="millisecondsTimeout">当前线程挂起指定的时间</param>
        /// <param name="isDoEvents">是否调用Application.DoEvents()</param> 
        public static void Sleep(int millisecondsTimeout, bool isDoEvents = false)
        {
            var dt = DateTime.Now.AddMilliseconds(millisecondsTimeout);

            do
            {
                Thread.Sleep(1);

                if (isDoEvents) Application.DoEvents();
            } while (DateTime.Now <= dt);
        }

        /// <summary>
        /// 启动一个背景线程，适合耗时任务
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="action">要执行的方法</param>
        /// <param name="state">包含方法所用数据的对象</param>
        /// <param name="apartmentState">单元状态</param>
        public static Thread StartThread<T>(Action<T> action, T state = default(T), ApartmentState apartmentState = ApartmentState.MTA)
        {
            var t = new Thread(o => action((T)o)) { IsBackground = true };

            t.TrySetApartmentState(apartmentState);
            t.Start(state);

            return t;
        }

        /// <summary>
        /// 启动一个背景线程,适合耗时任务
        /// </summary>
        /// <param name="action">要执行的方法</param>
        public static Thread StartThread(Action action)
        {
            return StartThread(delegate (object o) { action(); });
        }

        /// <summary>
        /// 向线程池中加入action，适合短时任务
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="action">要执行的方法</param>
        /// <param name="state">包含方法所用数据的对象</param>
        public static void QueueUserWorkItem<T>(Action<T> action, T state = default(T))
        {
            ThreadPool.QueueUserWorkItem(o => action((T)o), state);
        }

        /// <summary>
        /// 向线程池中加入action，适合短时任务
        /// </summary>
        /// <param name="action">要执行的方法</param>
        public static void QueueUserWorkItem(Action action)
        {
            QueueUserWorkItem(delegate (object o) { action(); });
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <typeparam name="T">包含方法所用数据的对象类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="func">要执行的方法</param>
        /// <param name="state">包含方法所用数据的对象</param>
        /// <param name="taskCreationOptions">任务创建选项</param>
        /// <returns>任务</returns>
        public static Task<TResult> StartTask<T, TResult>(Func<T, TResult> func, T state = default(T), TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
        {
            return Task.Factory.StartNew(o => func((T)o), state, taskCreationOptions);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="func">要执行的方法</param>
        /// <param name="taskCreationOptions">任务创建选项</param>
        /// <returns>任务</returns>
        public static Task<TResult> StartTask<TResult>(Func<TResult> func, TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
        {
            return StartTask<object, TResult>(o => func(), null, taskCreationOptions);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <typeparam name="T">包含方法所用数据的对象类型</typeparam>
        /// <param name="action">要执行的方法</param>
        /// <param name="state">包含方法所用数据的对象</param>
        /// <param name="taskCreationOptions">任务创建选项</param>
        /// <returns>任务</returns>
        public static Task StartTask<T>(Action<T> action, T state = default(T), TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
        {
            return Task.Factory.StartNew(o => action((T)o), state, taskCreationOptions);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="action">要执行的方法</param>
        /// <param name="taskCreationOptions">任务创建选项</param>
        /// <returns>任务</returns>
        public static Task StartTask(Action action, TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
        {
            return StartTask<object>(o => action(), null, taskCreationOptions);
        }
    }
}
