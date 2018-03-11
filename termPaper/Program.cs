using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace termPaper
{
    class Program
    {

        static void Main(string[] args)
        {
            int X = 30000000;
            if (args.Length == 1)
            {
                X = Int32.Parse(args[0]);
            }
            ParallelThreads.X = ParallelTasks.X = X;
            Helper.MAX_THREADS = 4;
            Helper.EXPERIMENT_NUMBER = 100;

            Console.WriteLine("Count in ONE Thread variable");
            Helper.MeasureAndShowOneThread(ParallelThreads.CountInOneThreadStaticVariable);
            Console.WriteLine("Count using Threads shared STATIC variable");
            Helper.MeasureAndShow(ParallelThreads.CountUsingThreadsShared);
            Console.WriteLine("Count using LOCKER");
            Helper.MeasureAndShow(ParallelThreads.CountUsingLock);
            Console.WriteLine("Count using MONITOR");
            Helper.MeasureAndShow(ParallelThreads.CountUsingMonitor);
            Console.WriteLine("Count using MONITOR TRY");
            Helper.MeasureAndShow(ParallelThreads.CountUsingTryMonitor);
            Console.WriteLine("Count Using INTERLOCK");
            Helper.MeasureAndShow(ParallelThreads.CountUsingInterLock);
            Console.WriteLine("Count using MUTEX");
            Helper.MeasureAndShow(ParallelThreads.CountUsingMutex);

            Console.WriteLine("Count using asyncron delegate with shared X");
            Helper.MeasureAndShow(ParallelTasks.CountUsingDelegateWithSharedX);

            Console.WriteLine("Count using Tasks with shared X");
            Helper.MeasureAndShow(ParallelTasks.CountUsingSharedTasks);



            Console.ReadKey();
        }


      
    }
}
