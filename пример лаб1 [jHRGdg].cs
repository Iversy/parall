using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Print()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }
        static void Main(string[] args)
        {
            Thread t = new Thread(Print);
            t.Start();
            t.Join();
            Console.WriteLine("Main");

            Thread[] threads = new Thread[4];
            for (int i = 0; i < threads.Length; i++)
            {
               threads[i] = new Thread(Print);
               threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
            {
               threads[i].Join();
            }
            Console.WriteLine("Main");

            int n = 200;
            int[] a = new int[n];
            // 111 222 333 444
            int m = 4;
            Thread[] threads = new Thread[4];
            DateTime d1 = DateTime.Now;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < threads.Length; i++) 
            {
                int start = n/m*i, end = n / m * (i+1), A = i + 1;
                threads[i] = new Thread(() =>
                {
                    for (int j = start; j < end; j++)
                    {
                        a[j] = A;
                    }
                });
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
            DateTime d2 = DateTime.Now;
            sw.Stop();
            var d = (d2 - d1).TotalMilliseconds;
            Console.WriteLine(d);
            Console.WriteLine(sw.ElapsedMilliseconds);
            for (int i = 0;i < a.Length; i++) 
            {
                Console.Write(a[i].ToString() + " ");
            }
        }
    }
}
