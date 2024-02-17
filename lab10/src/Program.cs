namespace Lab10;
using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public static class Program
{
    private static void Main()
    {
        var myConfig = new MyConfig();

        var nativeSettings = new NativeWindowSettings() {
            ClientSize = new Vector2i(800, 600),
            Title = "Lab 10 by Dmytro Naumov",
            Flags = ContextFlags.ForwardCompatible
        };

        var gwSettings = GameWindowSettings.Default;

        using (var window = new MainWindow(gwSettings, nativeSettings, myConfig)) {
            window.Run();
        }
        Console.WriteLine();
    }
}
