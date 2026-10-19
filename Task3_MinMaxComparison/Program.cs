using System;
using System.Diagnostics;
using System.Threading;

namespace Task3_MinMaxComparison
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[10000000];
            Random random = new Random(20);

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(-1000000, 1000001);
            }

            int threadCount = 4;
            int repeatCount = 5;
            int sequentialMin;
            int sequentialMax;
            int parallelMin;
            int parallelMax;

            // Первый запуск делаем до измерений.
            FindMinMaxSequential(numbers, out sequentialMin, out sequentialMax);
            FindMinMaxParallel(numbers, threadCount, out parallelMin, out parallelMax);

            double sequentialTotal = 0;
            double parallelTotal = 0;
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Задание 3. Поиск минимума и максимума массива");
            Console.WriteLine("Количество элементов: " + numbers.Length);
            Console.WriteLine("Количество потоков: " + threadCount);
            Console.WriteLine("Число измерений: " + repeatCount);
            Console.WriteLine();

            for (int i = 0; i < repeatCount; i++)
            {
                stopwatch.Restart();
                FindMinMaxSequential(numbers, out sequentialMin, out sequentialMax);
                stopwatch.Stop();
                double sequentialTime = stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Restart();
                FindMinMaxParallel(numbers, threadCount, out parallelMin, out parallelMax);
                stopwatch.Stop();
                double parallelTime = stopwatch.Elapsed.TotalMilliseconds;

                sequentialTotal += sequentialTime;
                parallelTotal += parallelTime;

                Console.WriteLine("Запуск " + (i + 1) + ": последовательно "
                    + sequentialTime.ToString("F2") + " мс, параллельно "
                    + parallelTime.ToString("F2") + " мс");

                if (sequentialMin != parallelMin || sequentialMax != parallelMax)
                {
                    Console.WriteLine("Ошибка: результаты не совпадают!");
                }
            }

            double sequentialAverage = sequentialTotal / repeatCount;
            double parallelAverage = parallelTotal / repeatCount;

            Console.WriteLine();
            Console.WriteLine("Минимум: " + sequentialMin);
            Console.WriteLine("Максимум: " + sequentialMax);
            Console.WriteLine("Среднее время последовательного поиска: "
                + sequentialAverage.ToString("F2") + " мс");
            Console.WriteLine("Среднее время параллельного поиска: "
                + parallelAverage.ToString("F2") + " мс");

            if (parallelAverage < sequentialAverage)
            {
                Console.WriteLine("В этом запуске параллельный способ оказался быстрее.");
            }
            else
            {
                Console.WriteLine("В этом запуске последовательный способ оказался быстрее или равен.");
            }
        }

        static void FindMinMaxSequential(int[] numbers, out int min, out int max)
        {
            min = numbers[0];
            max = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < min)
                {
                    min = numbers[i];
                }

                if (numbers[i] > max)
                {
                    max = numbers[i];
                }
            }
        }

        static void FindMinMaxParallel(int[] numbers, int threadCount, out int min, out int max)
        {
            Thread[] threads = new Thread[threadCount];
            int[] partMin = new int[threadCount];
            int[] partMax = new int[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int part = i;
                threads[i] = new Thread(() =>
                {
                    int start = part * numbers.Length / threadCount;
                    int end = (part + 1) * numbers.Length / threadCount;
                    int currentMin = numbers[start];
                    int currentMax = numbers[start];

                    for (int j = start + 1; j < end; j++)
                    {
                        if (numbers[j] < currentMin)
                        {
                            currentMin = numbers[j];
                        }

                        if (numbers[j] > currentMax)
                        {
                            currentMax = numbers[j];
                        }
                    }

                    partMin[part] = currentMin;
                    partMax[part] = currentMax;
                });

                threads[i].Start();
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            min = partMin[0];
            max = partMax[0];

            for (int i = 1; i < threadCount; i++)
            {
                if (partMin[i] < min)
                {
                    min = partMin[i];
                }

                if (partMax[i] > max)
                {
                    max = partMax[i];
                }
            }
        }
    }
}
