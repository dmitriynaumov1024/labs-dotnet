namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class InterpolatorTest
{
    public List<ValuedPoint> Points { get; set; }
    public List<Shape<ValuedPoint>> Shapes { get; set; }
    public List<Point> Targets { get; set; }
    public SortedDictionary<string, List<ValuedPoint>> Result { get; set; }

    public string Run (Func<Shape<ValuedPoint>, Interpolator> factory) 
    {
        this.Result ??= new();
        InterpolatorArray it = new InterpolatorArray(){ 
            Interpolators = this.Shapes.Select(factory).ToList() 
        };
        string name = it.Interpolators[0].GetType().Name;
        List<ValuedPoint> result = this.Points.ToList();
        foreach (var p in this.Targets) {
            result.Add(new ValuedPoint(p.X, p.Y, p.Z, it.GetValueAt(p)));
        }
        this.Result[name] = result;
        return name;
    }

    public static InterpolatorTest FromFile (TextReader reader)
    {
        char[] separators = new[] { ' ', '\t' };
        var splitOpts = StringSplitOptions.RemoveEmptyEntries;
        var result = new InterpolatorTest();
        result.Points = new();
        result.Shapes = new();
        result.Targets = new();
        result.Result = new(); 
        string currentMode = null;
        int i = 0;
        while (true) {
            i++;
            string line = reader.ReadLine()?.TrimStart();
            if (line == null) {
                break;
            }  
            else if (line.StartsWith("#") || line.Length < 2) {
                continue;
            }
            else if (line.StartsWith("[")) {
                currentMode = line.Trim().ToLower();
            }
            else if (currentMode == "[points]") {
                string[] words = line.Split(separators, splitOpts);
                if (words.Length < 4) {
                    throw new InputFormatException(i, "ValuedPoint expects 4 numbers (x, y, z, value)");
                }
                result.Points.Add (
                    new ValuedPoint (
                        double.Parse(words[0]),
                        double.Parse(words[1]),
                        double.Parse(words[2]),
                        double.Parse(words[3])
                    )
                );
            }
            else if (currentMode == "[shapes]") {
                string[] words = line.Split(separators, splitOpts);
                if (words.Length < 3) {
                    throw new InputFormatException(i, "Shape expects at least 3 verts");
                }
                result.Shapes.Add (
                    new Shape<ValuedPoint>(
                        words.Select(int.Parse)
                             .Select(index => result.Points[index])
                    )
                );
            }
            else if (currentMode == "[targets]") {
                string[] words = line.Split(separators, splitOpts);
                if (words.Length < 3) {
                    throw new InputFormatException(i, "Point expects at least 3 numbers (x, y, z)");
                }
                result.Targets.Add (
                    new Point (
                        double.Parse(words[0]),
                        double.Parse(words[1]),
                        double.Parse(words[2])
                    )
                );
            }
        }
        return result;
    }

    public void ToFile (TextWriter output)
    {
        output.Write($"# {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC\n");
        output.Write("\n[points]\n");
        foreach (var shape in this.Shapes) {
            foreach (var point in shape.Verts) {
                output.Write($"{point.X:F3} {point.Y:F3} {point.Z:F3} {point.Value:F3}\n");
            }
        }
        output.Write("\n[shapes]\n");
        int i=0;
        foreach (var shape in this.Shapes) {
            for (int j=0; j<shape.VertCount; j++) {
                output.Write($"{i+j} ");
            }
            output.Write("\n");
            i += shape.VertCount;
        }
        output.Write("\n[targets]\n");
        foreach (var point in this.Targets) {
            output.Write($"{point.X:F3} {point.Y:F3} {point.Z:F3}\n");
        }
        foreach (var (name, result) in this.Result) {
            output.Write($"\n[result.{name}]\n");
            foreach (var point in result) {
                output.Write($"{point.X:F3} {point.Y:F3} {point.Z:F3} {point.Value:F3}\n");
            }
        }
    }

    public void ToPythonFile (TextWriter output)
    {
        output.Write($"# {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC\n");
        output.Write("Sizes = [");
        for (int i=0; i<this.Points.Count; i++) {
            output.Write("3, ");
        }
        for (int i=0; i<this.Targets.Count; i++) {
            output.Write("1, ");
        }
        output.Write("]\n");
        
        output.Write("X = [");
        foreach (var point in this.Points) {
            output.Write($"{point.X:F3}, ");
        }
        foreach (var point in this.Targets) {
            output.Write($"{point.X:F3}, ");
        }
        output.Write("]\n");

        output.Write("Y = [");
        foreach (var point in this.Points) {
            output.Write($"{point.Y:F3}, ");
        }
        foreach (var point in this.Targets) {
            output.Write($"{point.Y:F3}, ");
        }
        output.Write("]\n");

        output.Write("Z = [");
        foreach (var point in this.Points) {
            output.Write($"{point.Z:F3}, ");
        }
        foreach (var point in this.Targets) {
            output.Write($"{point.Z:F3}, ");
        }
        output.Write("]\n");

        foreach (var (name, result) in this.Result) {
            output.Write($"Result{name} = [");
            foreach (var point in result) {
                output.Write($"{point.Value:F3}, ");
            }
            output.Write("]\n");
        }
    }
}
