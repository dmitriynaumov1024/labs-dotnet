namespace Lab01;
using System;

public class Program
{
    public static void Main (string[] args) 
    {
        Console.WriteLine("Наумов Дмитро Павлович, гр.8.1213");
        Console.WriteLine("Обчислення площі трикутника за формулою Герона");
        Console.WriteLine("");
        double a = 0, b = 0, c = 0;
        try {
            Console.Write("a = ");
            a = double.Parse(Console.ReadLine().Trim());
            Console.Write("b = ");
            b = double.Parse(Console.ReadLine().Trim());
            Console.Write("c = ");
            c = double.Parse(Console.ReadLine().Trim());
        }
        catch (Exception ex) {
            double example = 123.4567;
            Console.WriteLine($"Такі числа не підтримуються. Спробуйте {example}\n");
        }
        double p = (a + b + c) / 2.0;
        Console.WriteLine("");
        Console.WriteLine($"p = (a + b + c) / 2 = {p:F3}");
        double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        Console.WriteLine($"S = sqrt(p*(p-a)*(p-b)*(p-c)) = {S:F3}\n");
    }
}
