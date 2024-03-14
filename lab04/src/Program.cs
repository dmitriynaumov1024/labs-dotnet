namespace Lab04;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Program
{
    static readonly string usage = "Usage: <filename>";

    public static void Main (string[] args)
    {
        if (args.Length < 1) {
            Console.WriteLine(usage);
            return;
        }

        Console.WriteLine("Наумов Дмитро Павлович, гр.8.1213\n----------------------------");

        try {
            string text = File.ReadAllText(args[0]);
            TextStats stats = TextStats.For(text, new TextStatsConfig() { IgnoreCase = true });

            var words = stats.WordStats.OrderBy(kv => -kv.Value).Take(10).ToList();
            var punct = stats.PunctStats.OrderBy(kv => -kv.Value).Take(10).ToList();
            
            Console.WriteLine("Most common words:");
            foreach (var item in words) {
                Console.WriteLine($" {item.Key} {item.Value}");
            } 
            Console.WriteLine("----------------------------");
            Console.WriteLine("Most common punctuation:");
            foreach (var item in punct) {
                Console.WriteLine($" {item.Key} {item.Value}");
            }
            Console.WriteLine("----------------------------");
        } 
        catch (Exception ex) {
            Console.WriteLine(usage);
            Console.WriteLine(ex);
            return;
        }
    }
}
