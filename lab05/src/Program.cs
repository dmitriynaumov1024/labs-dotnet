namespace Lab05;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Program
{
    static readonly string usage = "Usage: <filename> <letter>";

    public static void Main (string[] args)
    {
        if (args.Length < 2) {
            Console.WriteLine(usage);
            return;
        }

        Console.WriteLine("Наумов Дмитро Павлович, гр.8.1213\n----------------------------");

        try {
            string text = File.ReadAllText(args[0]);
            char target = args[1][0];
            int count = text.Count(c => c == target);
            Console.WriteLine($"{args[0]} contains {count}x target \'{target}\'\n");
        } 
        catch (Exception ex) {
            Console.WriteLine(usage);
            Console.WriteLine(ex);
            return;
        }
    }
}
