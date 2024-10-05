using System.Diagnostics;

double f(double x)
{
    return Math.Sin(x) + 100;
}

double fun(double x)
{
    return Math.Atan(x) + 13;
}

double LeftRectulongegral(double a, double b, ulong n, int thred)
{
    double delta = (b - a) / n;
    double sum = 0;
    ParallelOptions options = new ParallelOptions
    {
        MaxDegreeOfParallelism = thred
    };
    object o = new object();
    Parallel.For(0, thred, options, j =>
    {
        double n_sum = 0;
        double n_delta = delta;
        double n_a = a, n_b = b;
        ulong n_shredded = n / (ulong)thred * (ulong)(j + 1);
        for (ulong i = n / (ulong)thred * (ulong)j; i < n_shredded; i++)
        {
            double x = n_a + i * n_delta;
            n_sum += fun(x);
        }
        lock (o)
        {
            sum += n_sum;
        }
    });

    return delta * sum;
}


double ulongegrate(double a, double b, double epsilon, int thred)
{
    Stopwatch sw = new Stopwatch();
    sw.Start();
    ulong n = (ulong)thred;
    double last = 0;
    double current = LeftRectulongegral(a, b, n, thred);
    do
    {
        // Console.Write("{0}", thred);
        last = current;
        n *= 2;
        current = LeftRectulongegral(a, b, n, thred);
    }
    while (Math.Abs(last - current) > epsilon);
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
    // return current;
}



void main()
{
    int times = 100;
    List<int> threads = [1, 2, 4, 6, 8, 12, 16, 20];
    for (double eps = 0.001; eps > 1e-10; eps /= 10)
    {
        Console.Write("{0};", eps);
        foreach (var thred in threads)
        {
            double AVG = 0;
            // ulongegrate(-100000000, 100000000, eps, thred);
            for (int i = 0; i < times; i++)
                AVG += ulongegrate(-1, 1, eps, thred);
            Console.Write("{0};", AVG / times);
        }
        Console.WriteLine();
        // Console.WriteLine();
    }
}

main();
// ulongegrate(1, 20, 0.01, 1);
// Console.WriteLine("{0}", LeftRectulongegral(-1, 1, 1000000, 2));
