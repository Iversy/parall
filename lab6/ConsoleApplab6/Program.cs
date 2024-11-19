using System.Threading;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;





class Program
{
    static int foodPortions = 8;
    static readonly object foodLock = new object();
    static bool isMotherCalled = false;

    static async Task Main(string[] args)
    {
        int n = 12;
        Task[] chicks = new Task[n];
        for (int i = 0; i < n; i++)
        {
            int chickId = i + 1;
            chicks[i] = Task.Run(() => Chick(chickId));
        }
        await Task.WhenAll(chicks);
    }

    static async Task Chick(int id)
    {
        Random random = new Random();
        while (true)
        {
            await Eat(id);
            await Task.Delay(random.Next(1000, 3000));
        }
    }

    static async Task Eat(int id)
    {
        while (true)
        {
            bool ate = false;
            lock (foodLock)
            {
                if (foodPortions > 0)
                {
                    foodPortions--;
                    Console.WriteLine($"[{id}] поел, осталось: {foodPortions} порций.");
                    ate = true;
                }
                else if (!isMotherCalled)
                {
                    Console.WriteLine($"[{id}] зовёт мать!");
                    isMotherCalled = true;
                    Task.Run(() => Mother());
                }
            }
            if (ate)
            {
                break;
            }
            await Task.Delay(100);
        }

    }

    static async Task Mother()
    {
        await Task.Delay(1000);
        lock (foodLock)
        {
            foodPortions = 8;
            Console.WriteLine("Мать положина есть!");
            isMotherCalled = false;
        }
    }


}
