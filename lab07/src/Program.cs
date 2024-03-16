namespace Lab07;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class Program
{
    // виконав Наумов Дмитро Павлович, гр.8.1213

    static readonly string usage = "Usage: <text> <directory> <filename patterns>";
    static readonly EnumerationOptions enumOptions = new() {
        RecurseSubdirectories = true
    };

    public static void Main (string[] args)
    {
        // Console.WriteLine("Наумов Дмитро Павлович, гр.8.1213");

        string text = null;
        string directory = null;
        string[] patterns = null;

        if (args.Length < 3) {
            Console.WriteLine(usage);
            return;
        }

        text = args[0].Trim();
        directory = Path.GetFullPath(args[1]);

        if (!Directory.Exists(directory)) {
            Console.WriteLine($"Directory {directory} not found.");
            return;
        }

        patterns = args[2].Split(new char[]{';', ','}, StringSplitOptions.RemoveEmptyEntries);

        var files = new SortedSet<string> (
            patterns.SelectMany (
                pattern => Directory.EnumerateFiles(directory, pattern, enumOptions)
            )
        );
        
        int lineCount = 0;
        int fileCount = 0;

        foreach (var filename in files) {
            int lineIndex = 0;
            bool fileMatch = false;
            foreach (var line in File.ReadLines(filename, Encoding.UTF8)) {
                lineIndex += 1;
                if (line.Contains(text)) {
                    lineCount++;
                    Console.WriteLine($"\u001b[01m{filename}\u001b[00m: line {lineIndex}:");
                    Console.WriteLine(line);
                    if (!fileMatch) {
                        fileCount += 1;
                        fileMatch = true;
                    }
                }
            }
        }

        Console.WriteLine($"\u001b[01mMatched {lineCount} {(lineCount == 1? "line" : "lines")} in {fileCount} {(fileCount==1? "file" : "files")}.\u001b[00m");
    }

}
