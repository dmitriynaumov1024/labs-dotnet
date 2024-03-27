namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

public class Program 
{
    public static void Main (string[] args) 
    {
        var itFactories = new Func<Shape<ValuedPoint>, Interpolator>[] {
            shape => new ProximityInterpolator(shape),
            shape => new NaiveInterpolator(shape),
            shape => new TetrahedronClassicInterpolator(shape).Build()
        };

        string inputPath = null;
        foreach (string arg in args) {
            if (arg.StartsWith("input=")) {
                inputPath = arg.Substring(6);
                break;
            }
        }

        if (inputPath == null) {
            Console.WriteLine("[i] Usage: input=<inputPath> output=<outputPath>...");
            return;
        }

        InterpolatorTest testCase;
        try {
            var reader = new StreamReader(File.OpenRead(inputPath));
            testCase = InterpolatorTest.FromFile(reader);
        }
        catch (InputFormatException ex) {
            Console.WriteLine("[x] {0}: {1}: {2}", ex.GetType().Name, inputPath, ex.Message);
            return;
        }
        catch (Exception ex) {
            Console.WriteLine("[x] {0}: {1}", ex.GetType().Name, ex.Message);
            return;
        }

        testCase.Targets = new();
        for (int x=-8; x<10; x+=2) {
            for (int y=-8; y<10; y+=2) {
                int z = 0;
                testCase.Targets.Add(new Point(x, y, z));
            }
        }

        foreach (var factory in itFactories) {
            try {
                testCase.Run(factory);
            }
            catch (Exception ex) {
                Console.WriteLine("[x] {0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }
        }

        foreach (string arg in args) {
            if (arg.StartsWith("output=")) {
                string outputPath = arg.Substring(7);
                try {
                    var writer = new StreamWriter(File.Create(outputPath));
                    if (outputPath.EndsWith(".py")) {
                        testCase.ToPythonFile(writer);
                    }
                    else {
                        testCase.ToFile(writer);
                    }
                    Console.WriteLine("[i] Write: {0}", outputPath);
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex) {
                    Console.WriteLine("[x] {0}: {1}", ex.GetType().Name, ex.Message);
                    return;
                }
            }
        }
    }
}
