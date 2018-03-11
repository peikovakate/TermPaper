using System;
using System.Collections.Generic;

namespace termPaper
{
    static class Helper
    {
        public static int MAX_THREADS = 4;
        public static int EXPERIMENT_NUMBER = 100;

        public static void MeasureAndShow(Func<int, int> MethodToMeasure)
        {
            for (int i = 1; i <= MAX_THREADS; i++)
            {
                long averageTime = 0;
                int x = 0;
                for (int j = 0; j < EXPERIMENT_NUMBER; j++)
                {
                    var data = MeasureTime(MethodToMeasure, i);
                    averageTime += data.Item1;
                    x = data.Item2;
                }
                averageTime /= EXPERIMENT_NUMBER;
                ShowResult(averageTime, x, i);
            }
        }

        public static void MeasureAndShowOneThread(Func<int, int> MethodToMeasure)
        {
            long averageTime = 0;
            int x = 0;
            for (int j = 0; j < EXPERIMENT_NUMBER; j++)
            {
                var data = MeasureTime(MethodToMeasure, 1);
                averageTime += data.Item1;
                x = data.Item2;
            }
            averageTime /= EXPERIMENT_NUMBER;
            ShowResult(averageTime, x, 1);
        }

        private static void ShowResult(long time, int x, int numberOfThreads)
        {
            Console.WriteLine("x = {0} threads = {1} time = {2}", x, numberOfThreads, time);
        }

        private static void ShowResult(Tuple<long, int, int> results)
        {
            Console.WriteLine("x = {0} threads = {1} time = {2}", results.Item2, results.Item3, results.Item1);
        }

        private static Tuple<long, int, int> MeasureTime(Func<int, int> MethodToMeasure, int numberOfThreads)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int value;
            value = MethodToMeasure(numberOfThreads);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Tuple.Create(elapsedMs, value, numberOfThreads);
        }

        private static List<int> divide(int n, int numberOfThreads)
        {
            int modulo = n % numberOfThreads;
            int z = n / numberOfThreads;
            List<int> sizes = new List<int>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                sizes.Add(z + (modulo-- > 0 ? 1 : 0));
            }
            return sizes;
        }
    }
}
