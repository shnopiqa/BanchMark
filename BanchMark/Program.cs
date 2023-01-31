using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;

public class Program
{
    public class ThreadStartVsThreadPoolQueueVsTaskRunBenchmark
    {
        [Benchmark(Baseline = true)]
        public void ThreadStart()
        {
            bool b = false;
            var thread = new Thread(() =>
            {
                b = true;
            });
            thread.Start();
            while (!Volatile.Read(ref b)) ;
        }
        [Benchmark]
        public void ThreadPollQueue()
        {

            bool b = false;
             ThreadPool.QueueUserWorkItem(o =>
            {
                b = true;
            });
            
            while (!Volatile.Read(ref b)) ;
        }
        [Benchmark]
        public void TaskRun()
        {

            bool b = false;
            Task.Run(() =>
            {
                b = true;
            });

            while (!Volatile.Read(ref b)) ;
        }
    }
    public class SleepVsDelayBenchmark
    {
        [Params(1, 5, 50)]
        public int Duration;
        [Benchmark]
        public void ThreadSleep() => Thread.Sleep(Duration);
        [Benchmark]
        public Task TaskDelay() => Task.Delay(Duration);
    }
    static void Main()
    {
        //BenchmarkRunner.Run<SleepVsDelayBenchmark>();
        BenchmarkRunner.Run<ThreadStartVsThreadPoolQueueVsTaskRunBenchmark>(DefaultConfig.Instance.AddColumn(StatisticColumn.P95));
    }
}
