using System.Threading;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


// неделимая рассылка
void mail(int n_threads)
{
    Thread[] threads = new Thread[n_threads];
    string buf = "";
    for (int i = 0; i < 10; ++i)
    {
        buf += i.ToString();
        for (int k = 0; k < n_threads; ++k)
        {
            threads[k] = new Thread(() =>
            {
                lock (buf)
                {
                    Console.WriteLine("[{0}]:{1}", Thread.GetCurrentProcessorId(), buf);
                }
            });
            threads[k].Start();
        }
        foreach (Thread thred in threads)
        {
            thred.Join();
        }
    }
}






void main()
{
    int n = 12;

    mail(n);


}



main();