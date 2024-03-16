namespace Lab06;
using System;
using System.Threading;
using System.Diagnostics;

public class SubProgram
{
    public Int64 N { get; private set; }
    public Int64 I0 { get; private set; }
    public Int64 Step { get; private set; }

    public Double Sum { get; private set; }
    public Int64 Time { get; private set; }

    public SubProgram (Int64 n, Int64 i0, Int64 step)
    {
        this.N = n;
        this.I0 = i0;
        this.Step = step;
    }

    public Double F (Double i)
    {
        return Math.Pow(i, 2) / (1.0 + Math.Pow(i, 4));
    }

    public void Run (Stopwatch timer)
    {
        this.Sum = 0;

        for (Int64 i = this.I0; i < this.N; i += this.Step) {
            this.Sum += this.F((double)i);
        }

        this.Time = timer.ElapsedMilliseconds;
    }
}
