namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class PbmOneParser : IImageParser<bool>
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string Content { get; set; }

    public static PbmOneParser File (string filename) 
    {
        var lines = System.IO.File.ReadAllLines(filename)
            .Where(line => !line.Contains("#")).ToList();
        var formatOK = lines[0].Trim().Equals("P1");
        if (!formatOK) {
            throw new Exception("Input file is in wrong format");
        }
        var result = new PbmOneParser();
        var dimensionLine = lines[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (dimensionLine.Length >= 2) {
            result.Width = int.Parse(dimensionLine[0]);
            result.Height = int.Parse(dimensionLine[1]);
        }
        else if (dimensionLine.Length == 1) {
            result.Width = int.Parse(dimensionLine[0]);
            result.Height = result.Width;
        }
        else {
            result.Width = 1;
            result.Height = 1;
        }

        result.Content = String.Join("", lines.Skip(2));
        return result;
    }

    public bool[][] Parse () 
    {
        var result = new bool[this.Height][];
        var items = this.Content
            .Where(c => c == '0' || c == '1')
            .Select(c => c == '1')
            .Chunk(this.Width);
        int i = 0;
        foreach (var line in items) {
            result[i++] = line;
            if (i == this.Height) break;
        }
        return result;
    }
}
