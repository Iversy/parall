using System.Diagnostics;

int N = 1000;

// var B = new double[N];



void Fill_Sequational(int[] A, double[] B)
{
    for (int i = 0; i < A.Length; i++)
    {
        for (int j = 0; j <= i; j++)
        {
            B[i] += Math.Pow(A[j], 1.768);
        }
    }
}




void Fill_Parallel(int[] A, double[] B, int threads_number)
{
    Thread[] threads = new Thread[threads_number];
    for (int part = 0; part < threads_number; part++)
    {
        int start = part * A.Length / threads_number;
        int end = (part + 1) * A.Length / threads_number;
        threads[part] = new Thread(() =>
        {
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    B[i] += Math.Pow(A[j], 1.789);
                }
            }
        });
        threads[part].Start();
    }
    for (int part = 0; part < threads_number; part++)
    {
        threads[part].Join();
    }

}


double SeqTest(int[] A)
{
    Stopwatch sw = new Stopwatch();
    var B = new double[N];
    sw.Start();
    Fill_Sequational(A, B);
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
}

double ParTest(int[] A, int threads_number)
{
    Stopwatch sw = new Stopwatch();
    var B = new double[N];
    sw.Start();
    Fill_Parallel(A, B, threads_number);
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
}

void ParTesting(int[] A, int M)
{
    Console.WriteLine("Par Multi Test");
    int[] thread_number = [1, 2, 4, 8, 12];

    for (int number = 0; number < thread_number.Length; number++)
    {
        Console.WriteLine("Thread number = {0}", thread_number[number]);
        double minimum = int.MaxValue, maximum = -1, AVG = 0;
        for (int i = 0; i < M; i++)
        {
            double temp = ParTest(A, thread_number[number]);
            minimum = Math.Min(minimum, temp);
            maximum = Math.Max(maximum, temp);
            AVG += temp;
        }
        Console.WriteLine("Минимум:{0}, Максимум:{1}, Среднее:{2}", minimum, maximum, AVG / M);
    }

}



void Main()
{
    // Console.WriteLine("{0} миллисекунд", SeqTest());
    // Console.WriteLine("{0} секунд", ParTest(threads_number));
    int M = 10;
    var A = Enumerable.Range(0, N).ToArray();
    // Seq Multi Test
    Console.WriteLine("Seq Multi Test");
    double minimum1 = int.MaxValue, maximum1 = -1, AVG1 = 0;
    for (int i = 0; i < M; i++)
    {
        double temp = SeqTest(A);
        minimum1 = Math.Min(minimum1, temp);
        maximum1 = Math.Max(maximum1, temp);
        AVG1 += temp;
    }
    Console.WriteLine("Минимум:{0}", minimum1);
    Console.WriteLine("Максимум:{0}", maximum1);
    Console.WriteLine("Среднее:{0}", AVG1 / M);

    ParTesting(A, M);





}

Main();