namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public static class Program
{
    private static void Main()
    {
        var jsonOptions = new JsonSerializerOptions() {
            AllowTrailingCommas = true,
            WriteIndented = true
        };
        var myConfig = JsonSerializer.Deserialize<MyConfig>(File.ReadAllText("./src-config/config.json"), jsonOptions);
        // Console.WriteLine(JsonSerializer.Serialize(myConfig, jsonOptions));

        using (var window = MainWindow.Create(myConfig)) {
            window.Run();
        }
        Console.WriteLine();
    }
}
