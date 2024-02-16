namespace Lab10;
using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public static class Program
{
    private static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings() {
            ClientSize = new Vector2i(800, 600),
            Title = "Lab 10 by Dmytro Naumov",
            Flags = ContextFlags.ForwardCompatible
        };

        using (var window = new MainWindow(GameWindowSettings.Default, nativeWindowSettings)) {
            window.Run();
        }
        Console.WriteLine();
    }
}
