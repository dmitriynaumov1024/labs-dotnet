namespace Lab02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

public delegate Interpolator InterpolatorFactory(Point[] points, double[] values);

public class Program 
{
    static InterpolatorFactory[] itFactories = new InterpolatorFactory[] {
        (points, values)=> new NaiveTetrahedronInterpolator(points, values),
        (points, values)=> new ProximityTetrahedronInterpolator(points, values),
        (points, values)=> new ClassicTetrahedronInterpolator(points, values).Build()
    };

    static void TestInterpolators ()
    {
        Point[] points = new Point[] {
            new Point(6.0, 0.0, 0.0),
            new Point(-2.0, 0.0, 0.0),
            new Point(3.0, 4.0, 0.0),
            new Point(-3.0, 2.0, 4.2)
        };

        double[] values = new double[] {
            4.0, -1.1, 5.2, 9.0
        };

        Point[] targets = new Point[17 * 17 * 17];
        
        int i=0;
        for (int x=-8; x<=8; x++) {
            for (int y=-8; y<=8; y++) {
                for (int z=-8; z<=8; z++) {
                    targets[i] = new Point(x, y, z);
                    i++;
                }
            }
        }

        StreamWriter output = new StreamWriter(File.OpenWrite("./var/out.txt"));

        output.Write("X = [");
        for (i=0; i<targets.Length; i++) {
            output.Write($"{targets[i].X:F2}, ");
        }
        output.Write("]\n");

        output.Write("Y = [");
        for (i=0; i<targets.Length; i++) {
            output.Write($"{targets[i].Y:F2}, ");
        }
        output.Write("]\n");

        output.Write("Z = [");
        for (i=0; i<targets.Length; i++) {
            output.Write($"{targets[i].Z:F2}, ");
        }
        output.Write("]\n");

        foreach (var itFactory in itFactories) {
            Interpolator it = itFactory.Invoke(points, values);
            output.Write(it.GetType().Name);
            output.Write(" = [");
            foreach (var t in targets) {
                double val = it.GetValueAt(t);
                if (Double.IsNaN(val)) val = 0;
                output.Write($"{val:F3}, ");
            }
            output.Write("]\n");
        }

        output.Flush();
        output.Close();
    }

    public static void Main (string[] args) 
    {
        TestInterpolators();
    }
}
