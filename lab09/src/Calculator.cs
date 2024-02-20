namespace Lab09;
using System;

public class Calculator
{
    public string A { get; set; }
    public string B { get; set; }
    public string C { get; set; }

    public string S { get; private set; }

    public void Calculate()
    {
        try {
            double a = double.Parse(A);
            double b = double.Parse(B);
            double c = double.Parse(C);
            double p = (a + b + c) / 2.0;
            double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            if (double.IsNaN(s)) S = "Error: Not a triangle";
            else S = $"{s:F5}";
        }
        catch (Exception) {
            S = "Error: Wrong input format";
        }
    }
}
