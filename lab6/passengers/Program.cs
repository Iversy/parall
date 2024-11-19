using System;
using System.Threading;

class Program
{
    static Semaphore semaphore;
    static Mutex mutex;
    static int passengersCount = 0;
    static int totalPassengers;
    static int capacity;

    static void Main(string[] args)
    {
        totalPassengers = 12;
        capacity = 4;

        semaphore = new Semaphore(0, capacity);
        mutex = new Mutex();

        for (int i = 0; i < totalPassengers; i++)
        {
            new Thread(Passenger).Start(i);
        }


        new Thread(Train).Start();
    }

    static void Passenger(object id)
    {
        int passengerId = (int)id;
        Console.WriteLine($"Пассажир {passengerId} ожидает в очереди.");

        while (true)
        {

            mutex.WaitOne();
            if (passengersCount < capacity)
            {
                passengersCount++;
                Console.WriteLine($"Пассажир {passengerId} сел в вагончик. Текущее количество пассажиров: {passengersCount}.");
                if (passengersCount == capacity)
                {

                    semaphore.Release();
                }
                mutex.ReleaseMutex();
                break;
            }
            mutex.ReleaseMutex();
            Thread.Sleep(100);
        }
    }

    static void Train()
    {
        while (true)
        {

            semaphore.WaitOne();
            Console.WriteLine("Вагончик уехал с пассажирами!");


            Thread.Sleep(2000);


            mutex.WaitOne();
            Console.WriteLine("Вагончик вернулся. Пассажиры вышли.");
            passengersCount = 0;
            mutex.ReleaseMutex();
        }
    }
}
