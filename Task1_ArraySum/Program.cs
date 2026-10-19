using System;
using System.Threading;

namespace Task1_ArraySum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[5000000];
            Random random = new Random(10);

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(1, 101);
            }

            int threadCount = 4;
            Thread[] threads = new Thread[threadCount];
            long[] partSums = new long[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int part = i;

                threads[i] = new Thread(() =>
                {
                    int start = part * numbers.Length / threadCount;
                    int end = (part + 1) * numbers.Length / threadCount;
                    long sum = 0;

                    for (int j = start; j < end; j++)
                    {
                        sum += numbers[j];
                    }

                    partSums[part] = sum;
                });

                threads[i].Start();
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            long totalSum = 0;

            for (int i = 0; i < partSums.Length; i++)
            {
                totalSum += partSums[i];
            }

            Console.WriteLine("Сумма элементов массива: " + totalSum);
        }
    }
}
