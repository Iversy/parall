using System.Diagnostics;
using System.Net.Http.Headers;

// int N = 1000;

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

void Prefix_Fill(int[] A, double[] B, int thread_number)
{
    // if (thread_number == 1)
    // {
    //     for (int i = 0; i < A.Length; i++)
    //     {
    //         for (int j = 0; j <= i; j++)
    //         {
    //             B[i] += Math.Pow(A[j], 1.789);
    //         }
    //     }
    //     return;
    // }
    Thread[] threads = new Thread[thread_number];
    for (int number = 0; number < thread_number; number++)
    {
        int start = number * A.Length / thread_number;
        int end = (number + 1) * A.Length / thread_number;
        threads[number] = new Thread(() =>
        {
            int localStart = start;
            int localEnd = end;
            for (int i = localStart; i < localStart; i++)
            {
                B[i] = Math.Pow(A[i], 1.789);
            }
        });

    }
    for (int i = 1; i < A.Length; i++)
        B[i] += B[i - 1];


}




void Fill_Parallel(int[] A, double[] B, int threads_number)
{
    Thread[] threads = new Thread[threads_number];
    for (int part = 0; part < threads_number; part++)
    {
        // int start = part * A.Length / threads_number;
        // int end = (part + 1) * A.Length / threads_number;
        threads[part] = new Thread(() =>
        {
            // for (int i = start; i < end; i++)
            // {
            //     for (int j = 0; j <= i; j++)
            //     {
            //         B[i] += Math.Pow(A[j], 1.789);
            //     }
            // }
            for (int i = part; i < A.Length; i += threads_number)
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


double SeqTest(int[] A, int N)
{
    Stopwatch sw = new Stopwatch();
    var B = new double[N];
    sw.Start();
    Fill_Sequational(A, B);
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
}

double ParTest(int[] A, int threads_number, int N)
{
    Stopwatch sw = new Stopwatch();
    var B = new double[N];
    sw.Start();
    Fill_Parallel(A, B, threads_number);
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
}

double otherTest(int[] A, int threads_number, int N)
{
    Stopwatch sw = new Stopwatch();
    var B = new double[N];
    sw.Start();
    Prefix_Fill(A, B, threads_number);
    sw.Stop();
    return sw.Elapsed.TotalSeconds;

}

void ParTesting(int[] A, int M, int N)
{
    // Console.WriteLine("Par Multi Test");
    int[] thread_number = [1, 2, 4, 8, 12, 16, 20];

    for (int number = 0; number < thread_number.Length; number++)
    {
        // Console.WriteLine("Thread number = {0}", thread_number[number]);
        double minimum = int.MaxValue, maximum = -1, AVG = 0;
        for (int i = 0; i < M; i++)
        {
            double temp = otherTest(A, thread_number[number], N);
            // minimum = Math.Min(minimum, temp);
            // maximum = Math.Max(maximum, temp);
            AVG += temp;
        }
        Console.Write("{0} ", AVG / M);
    }
    Console.WriteLine();
}



void Main()
{
    // Console.WriteLine("{0} миллисекунд", SeqTest());
    // Console.WriteLine("{0} секунд", ParTest(threads_number));
    int M = 100;

    // Seq Multi Test
    // Console.WriteLine("Seq Multi Test");
    // double minimum1 = int.MaxValue, maximum1 = -1, AVG1 = 0;
    // for (int i = 0; i < M; i++)
    // {
    //     double temp = SeqTest(A);
    //     minimum1 = Math.Min(minimum1, temp);
    //     maximum1 = Math.Max(maximum1, temp);
    //     AVG1 += temp;
    // }
    // Console.WriteLine("Минимум:{0}", minimum1);
    // Console.WriteLine("Максимум:{0}", maximum1);
    // Console.WriteLine("Среднее:{0}", AVG1 / M);

    for (int N = 100000; N < 2100001; N += 400000)
    {
        var A = Enumerable.Range(0, N).ToArray();
        ParTesting(A, M, N);
    }



}

Main();