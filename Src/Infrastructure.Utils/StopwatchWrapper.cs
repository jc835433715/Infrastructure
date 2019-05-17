using System;
using System.Diagnostics;

namespace Infrastructure.Utils
{
    /// <summary>
    /// Stopwatch包装器
    /// </summary>
    public class StopwatchWrapper : IDisposable
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public StopwatchWrapper() : this(string.Empty) { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="processName">过程名</param>
        public StopwatchWrapper(string processName)
        {
            this.id = sn++;
            this.methodName = processName;
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();

            currentForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"---{DateTime.Now:MM-dd HH:mm:ss:fff} 过程ID：{id:00000000000} 开始执行过程：{processName }");
            Console.ForegroundColor = currentForegroundColor;
        }

        /// <summary>
        /// 总运行时间
        /// </summary>
        public TimeSpan Elapsed => stopwatch.Elapsed;

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Dispose()
        {
            this.stopwatch.Stop();

            currentForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"...{DateTime.Now:MM-dd HH:mm:ss:fff} 过程ID：{id:00000000000} 完成执行过程：{methodName },耗时(ms)：{Elapsed.TotalMilliseconds.ToString("F3") }");
            Console.ForegroundColor = currentForegroundColor;
        }

        private readonly uint id;
        private static volatile uint sn;
        private ConsoleColor currentForegroundColor;
        private Stopwatch stopwatch;
        private readonly string methodName;
    }
}
