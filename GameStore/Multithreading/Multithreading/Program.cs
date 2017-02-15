using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace Multithreading
{
    class Program
    {
        private static readonly string text = "words.txt";
        static ConcurrentQueue<char> Read(string path)
        {
            var queue = new ConcurrentQueue<char>();
            using (var reader = new StreamReader(path))
            {
                while (reader.Peek() >= 0)
                {

                    char c = (char)reader.Read();
                    queue.Enqueue(c);
                }
            };
            return queue;
        }

        static void Main(string[] args)
        {
            var t1 = Read(text);
            var t2 = Read(text);
            var t3 = Read(text);
            var timer = new Stopwatch();
            timer.Start();
            var resultMulti = new Multithreading().Calculate(t1);
            timer.Stop();
            var multiTime = timer.ElapsedMilliseconds/1000.0;

            var timer2 = new Stopwatch();
            timer2.Start();
            new Singlethreading().Calculate(t2);
            timer2.Stop();
            var singleTime = timer2.ElapsedMilliseconds/1000.0;

            foreach (var pair in resultMulti.Result)
            {
                Console.WriteLine("  Char:{0} |   Occurrence count: {1} |Occurrence percentage {2:0.00}%  ", pair.Key, pair.Value, (pair.Value * 100.0d / t3.Count));
            }

            Console.WriteLine("\nTotal characters: {0}", t3.Count);
            Console.WriteLine("Elapsed time in multithreading: {0} seconds", multiTime);
            Console.WriteLine("Elapsed time in singlethreading: {0} seconds", singleTime);
            Console.ReadKey();
        }
    }
}
