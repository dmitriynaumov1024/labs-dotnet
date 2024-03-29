namespace Lab06;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static void Main (string[] args)
    {
        Console.WriteLine("Наумов Дмитро Павлович, гр.8.1213");
        Console.WriteLine("Багатопотокові обчислення");

        Int64 N = 0;
        Int32 Threads = 0;

        try {
            N = Int64.Parse(args[0]);
            Threads = Int32.Parse(args[1]);
            if (Threads < 1) Threads = 1;
            if (Threads > 16) Threads = 16;
        }
        catch (Exception) {
            Console.WriteLine("Usage: <N> <Threads>");
            return;
        }

        // ThreadPool.SetMaxThreads(11, 1);

        Console.WriteLine($"- N: {N}");
        Console.WriteLine($"- Threads: {Threads}");
        
        var subPrograms = Enumerable.Range(0, Threads)
            .Select(i => new SubProgram(N, i, Threads))
            .ToArray();

        var timer = Stopwatch.StartNew();
        
        var threads = new Task[Threads];

        for (Int32 i=0; i<Threads; i++) {
            var prog = subPrograms[i];
            threads[i] = new Task(()=> prog.Run(timer));
            threads[i].Start();
        }

        for (Int32 i=0; i<Threads; i++) {
            threads[i].Wait();
            Console.WriteLine($"- Time[{i}]: {subPrograms[i].Time}ms");
        }

        Double sum = subPrograms.Sum(prog => prog.Sum);
        Console.WriteLine($"- Time total: {timer.ElapsedMilliseconds}ms");
        Console.WriteLine($"- Sum: {sum:F9}");
    }
}
