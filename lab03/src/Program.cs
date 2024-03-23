namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

public delegate Interpolator InterpolatorFactory(Shape<ValuedPoint> shape);

public class Program 
{
    static InterpolatorFactory[] itFactories = new InterpolatorFactory[] {
        shape => new ProximityInterpolator(shape),
        shape => new NaiveInterpolator(shape),
        shape => new TetrahedronClassicInterpolator(shape).Build()
    };

    static void TestInterpolators ()
    {
        ValuedPoint[] points = new ValuedPoint[] {
            new ValuedPoint(6.0, 0.0, 0.1, 4.0),
            new ValuedPoint(-2.0, 0.0, 0.0, -1.1),
            new ValuedPoint(3.0, 4.0, -0.3, 5.2),
            new ValuedPoint(2.5, 2.5, 4.2, 9.0)
        };

        Shape<ValuedPoint> shape = new Shape<ValuedPoint>(points);

        Point[] targets = new Point[10 * 10 * 3];
        
        int i=0;
        for (int x=-3; x<=6; x++) {
            for (int y=-3; y<=6; y++) {
                targets[i++] = new Point(x, y, 0);
                targets[i++] = new Point(x, 2, y);
                targets[i++] = new Point(2, y, x);
            }
        }

        StreamWriter output = new StreamWriter(File.OpenWrite("./var/out.txt"));

        output.Write("X = [");
        for (i=0; i<points.Length; i++) {
            output.Write($"{points[i].X:F2}, ");
        }
        for (i=0; i<targets.Length; i++) {
            output.Write($"{targets[i].X:F2}, ");
        }
        output.Write("]\n");

        output.Write("Y = [");
        for (i=0; i<points.Length; i++) {
            output.Write($"{points[i].Y:F2}, ");
        }
        for (i=0; i<targets.Length; i++) {
            output.Write($"{targets[i].Y:F2}, ");
        }
        output.Write("]\n");

        output.Write("Z = [");
        for (i=0; i<points.Length; i++) {
            output.Write($"{points[i].Z:F2}, ");
        }
        for (i=0; i<targets.Length; i++) {
            output.Write($"{targets[i].Z:F2}, ");
        }
        output.Write("]\n");

        output.Write("Sizes = [");
        for (i=0; i<points.Length; i++) {
            output.Write("3, ");
        }
        for (i=0; i<targets.Length; i++) {
            output.Write("1, ");
        }
        output.Write("]\n");

        foreach (var itFactory in itFactories) {
            Interpolator it = itFactory.Invoke(shape);
            output.Write($"{it.GetType().Name} = [");
            foreach (var p in points) {
                output.Write($"{p.Value}, ");
            }
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
