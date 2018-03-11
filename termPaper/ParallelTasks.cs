using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace termPaper
{
    class ParallelTasks
    {
        public static int X;

        private static int CountPrivateX(int n)
        {
            int x = 0;
            for (int i = 0; i < n; i++)
            {
                x++;
            }
            return x;
        }

        private static void CountSharedStaticX(int n)
        {

            for (int i = 0; i < n; i++)
            {
                SharedX++;
            }
            //return x;
        }


        #region Simple delegate using static shared var
        private static int SharedX;

        private delegate void CountSharedXHandler(int n);
        public static int CountUsingDelegateWithSharedX(int numberOfThreads = 1)
        {
            SharedX = 0;
            List<CountSharedXHandler> handlers = new List<CountSharedXHandler>();
            List<IAsyncResult> results = new List<IAsyncResult>();
            int threadN = X / numberOfThreads;
            for (int i = 0; i < numberOfThreads; i++)
            {
                //    CountPrivateXHandler handler = new CountPrivateXHandler(CountPrivateX);
                CountSharedXHandler handler = new CountSharedXHandler(CountSharedX);
                IAsyncResult resultObj = handler.BeginInvoke(threadN, null, null);
                handlers.Add(handler);
                results.Add(resultObj);
            }
            for (int i = 0; i < numberOfThreads; i++)
            {
                handlers[i].EndInvoke(results[i]);
            }
            return SharedX;
        }

        private static void CountSharedX(int n)
        {
            for (int i = 0; i < n; i++)
            {
                //SharedX++;
                Interlocked.Increment(ref SharedX);
            }
        }


        #endregion

        #region Simple delegate using private and static shared var


        private delegate int CountPrivateXHandler(int n);
        //private delegate void CountSharedXHandler(int n);
        public static int CountUsingDelegate(int numberOfThreads = 1)
        {
            int x = 0;
            List<CountPrivateXHandler> handlers = new List<CountPrivateXHandler>();
            List<IAsyncResult> results = new List<IAsyncResult>();
            int threadN = X / numberOfThreads;
            for (int i = 0; i < numberOfThreads; i++)
            {
                CountPrivateXHandler handler = new CountPrivateXHandler(CountPrivateX);
                //CountSharedXHandler handler = new CountPrivateXHandler(CountSharedStaticX);
                IAsyncResult resultObj = handler.BeginInvoke(threadN, null, null);
                handlers.Add(handler);
                results.Add(resultObj);
            }
            for (int i = 0; i < numberOfThreads; i++)
            {
                x += handlers[i].EndInvoke(results[i]);
            }
            return x;
            //for (int i = 0; i < numberOfThreads; i++)
            //{
            //    handlers[i].EndInvoke(results[i]);
            //}
            //return SharedX;
        }



        #endregion

        #region TASKS
        public static int CountUsingTasks(int numberOfThreads)
        {

            List<Task<int>> tasks = new List<Task<int>>();
            int threadN = X / numberOfThreads;
            for (int i = 0; i < numberOfThreads; i++)
            {
                Task<int> task = new Task<int>(() => CountPrivateX(threadN));
                task.Start();
                tasks.Add(task);
            }
            int x = 0;
            for (int i = 0; i < numberOfThreads; i++)
            {
                x += tasks[i].Result;
            }

            return x;
        }
        #endregion

        #region Tasks Shared

        private static int CountSharedX2(int n)
        {
            for (int i = 0; i < n; i++)
            {
                //SharedX++;
                Interlocked.Increment(ref SharedX);
            }
            return 0;
        }

        public static int CountUsingSharedTasks(int numberOfThreads)
        {
            SharedX = 0;
            List<Task<int>> tasks = new List<Task<int>>();
            int threadN = X / numberOfThreads;
            for (int i = 0; i < numberOfThreads; i++)
            {
                Task<int> task = new Task<int>(() => CountSharedX2(threadN));
                task.Start();
                tasks.Add(task);
            }
            int x = 0;
            for (int i = 0; i < numberOfThreads; i++)
            {
                x += tasks[i].Result;
            }

            return SharedX;
        }

        #endregion

    }
}
