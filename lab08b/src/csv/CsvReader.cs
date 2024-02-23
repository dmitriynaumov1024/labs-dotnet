namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class CsvReader<TOutput> 
where TOutput : class
{
    public Func<string[], TOutput> ParseRow { get; set; }

    public CsvReader (Func<string[], TOutput> parseRow) 
    {
        this.ParseRow = parseRow;
    }

    public IEnumerable<TOutput> Parse (IEnumerable<string> source, CsvOptions options = null)
    {
        source = source.Select(line => line.Trim()).Where(line => line.Length > 0 && !line.StartsWith("#"));

        if (options?.SkipFirstRow == true) source = source.Skip(1);

        var splitLines = source.Select(line => line.Split(';', StringSplitOptions.None));
        
        var items = (options?.IgnoreErrors == true) ? 
            splitLines.Select(line => this.TryParseRow(line)) :
            splitLines.Select(line => this.ParseRow(line));
    
        return (options?.IgnoreNulls == true) ?
            items.Where(item => item != null).ToList() :
            items.ToList();
    }

    public IEnumerable<TOutput> Parse (TextReader reader, CsvOptions options = null)
    {
        var lines = new List<string>();
        while (true) {
            var line = reader.ReadLine();
            if (line == null) break;
            lines.Add(line);
        }
        return this.Parse(lines, options);
    }

    public TOutput TryParseRow (string[] line)
    {
        try {
            return this.ParseRow(line);
        }
        catch (Exception) {
            return null;
        }
    }
}
