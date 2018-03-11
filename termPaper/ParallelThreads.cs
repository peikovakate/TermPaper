using System.Collections.Generic;
using System.Threading;

namespace termPaper
{
    class ParallelThreads
    {
        public static int X;
        private static int sharedX = 0;
        private static int Func(object obj)
        {
            int count = 0;
            int n = (int)obj;
            for (int i = 0; i < n; i++)
            {
                count++;
            }
            return count;
        }

        private static void CountSharedVar(object obj)
        {
            int n = (int)obj;
            for (int i = 0; i < n; i++)
            {
                sharedX++;
            }
        }
        /*
        #region IN ONE THREAD 
        public static int CountInMainThread(int numberOfThreads = 1)
        {
            int x = 0;
            for (int i = 0; i < N; i++)
            {
                x++;
            }
            return x;
        }
        #endregion
        */

        #region IN ONE THREAD STATIC VARIABLE
        public static int CountInOneThreadStaticVariable(int numberOfThreads = 1)
        {
            sharedX = 0;
            for (int i = 0; i < X; i++)
            {
                sharedX++;
            }
            return sharedX;
        }
        #endregion

        #region Simple Threads with shared static x

        public static int CountUsingThreadsShared(int numberOfThreads)
        {
            sharedX = 0;
            int threadN = X / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CountSharedVar));
                threads.Add(thread);
                thread.Start(threadN);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            return sharedX;
        }

        private static void CountShared(object obj)
        {
            sharedX += Func(obj);
        }
        #endregion

        #region THREADS with LOCK
        static object locker = new object();
        public static int CountUsingLock(int numberOfThreads)
        {
            sharedX = 0;
            locker = new object();
            int threadN = X / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.Thread thread =
                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CountSharedWithLock));
                threads.Add(thread);
                thread.Start(threadN);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            return sharedX;
        }

        private static void CountSharedWithLock(object obj)
        {
            int n = (int)obj;
            for (int i = 0; i < n; i++)
            {
                lock (locker)
                {
                    sharedX++;
                }
            }
        }
        #endregion

        #region THREADS with MONITOR
        static object monitorLockerTRY = new object();
        public static int CountUsingTryMonitor(int numberOfThreads)
        {
            sharedX = 0;
            int threadN = X / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CountMonitorTry));
                threads.Add(thread);
                thread.Start(threadN);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            return sharedX;
        }

        private static void CountMonitorTry(object obj)
        {
            int n = (int)obj;
            for (int i = 0; i < n; i++)
            {
                Monitor.Enter(monitorLockerTRY);
                try
                {
                    sharedX++;
                }
                finally
                {
                    Monitor.Exit(monitorLockerTRY);
                }
            }

        }
        #endregion

        #region MONITOR no try
        static object monitorLocker = new object();
        public static int CountUsingMonitor(int numberOfThreads)
        {
            sharedX = 0;
            int threadN = X / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CountMonitor));
                threads.Add(thread);
                thread.Start(threadN);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            return sharedX;
        }

        private static void CountMonitor(object obj)
        {
            int n = (int)obj;
            for (int i = 0; i < n; i++)
            {
                Monitor.Enter(monitorLocker);
                sharedX++;
                Monitor.Exit(monitorLocker);
            }
        }
        #endregion

        #region MUTEX
        public static int CountUsingMutex(int numberOfThreads)
        {
            sharedX = 0;
            int threadN = X / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(mutexCount));
                threads.Add(thread);
                thread.Start(threadN);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            return sharedX++;
        }

        static Mutex mutexObj = new Mutex();
        private static void mutexCount(object size)
        {
            int n = (int)size;
           
            for (int i = 0; i < n; i++)
            {
                mutexObj.WaitOne();
                sharedX++;
                mutexObj.ReleaseMutex();
            }
           

        }
        #endregion

        #region InterLock
        public static int CountUsingInterLock(int numberOfThreads)
        {
            sharedX = 0;
            int threadN = X / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CountInterLock));
                threads.Add(thread);
                thread.Start(threadN);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            return sharedX;
        }

        private static void CountInterLock(object obj)
        {
            int count = Func(obj);
            int n = (int)obj;
            for (int i = 0; i < n; i++)
            {
                Interlocked.Increment(ref sharedX);

            }
        }
        #endregion

    }
}
