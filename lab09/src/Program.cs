namespace Lab09;
using System;
using Eto;
using Eto.Forms;
using Eto.Drawing;

public class Program
{
    [STAThread]
    public static void Main (string[] args)
    {
        // first, we have to init application, otherwise runtime error
        var app = new Application(Eto.Platform.Detect);

        // then we can create gui forms
        var form = new MainForm();

        // run app
        app.Run(form);
    }
}
