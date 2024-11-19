using System;
using System.Threading;

class Program
{
    static SemaphoreSlim potSemaphore = new SemaphoreSlim(0);
    static int honeyPot = 0;
    static int maxHoney;
    static Random random = new Random();

    static void Bee(int id)
    {
        while (true)
        {
            Thread.Sleep(random.Next(100, 500));
            Console.WriteLine($"Пчела {id} принесла порцию меда.");

            lock (potSemaphore)
            {
                honeyPot++;
                if (honeyPot == maxHoney)
                {
                    Console.WriteLine($"Пчела {id} принесла последнюю порцию меда и разбудила медведя!");
                    potSemaphore.Release();
                }
                else
                {
                    potSemaphore.Release();
                }
            }
        }
    }

    static void Bear()
    {
        while (true)
        {
            potSemaphore.Wait();

            Console.WriteLine("Медведь проснулся и ест мед.");
            Thread.Sleep(1000);

            lock (potSemaphore)
            {
                honeyPot = 0;
                Console.WriteLine("Медведь закончил есть мед и снова засыпает.");
            }
        }
    }

    static void Main(string[] args)
    {
        int n = 12;
        maxHoney = 48;

        Thread bearThread = new Thread(Bear);
        bearThread.Start();

        Thread[] bees = new Thread[n];
        for (int i = 0; i < n; i++)
        {
            bees[i] = new Thread(() => Bee(i + 1));
            bees[i].Start();
        }

        bearThread.Join();
        foreach (var bee in bees)
        {
            bee.Join();
        }
    }
}
