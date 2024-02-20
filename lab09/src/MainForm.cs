namespace Lab09;
using System;
using Eto;
using Eto.Forms;
using Eto.Drawing;

public class MainForm : Form
{
    public Calculator Calculator { get; set; }

    public MainForm() : base()
    {
        this.Title = "Lab 09";
        this.ClientSize = new Size(300, 400);
        this.MinimumSize = new Size(300, 350);

        this.Calculator = new Calculator();

        this.InitMenu();
        this.InitContent();
    }

    public void InitMenu()
    {
        this.Menu = new MenuBar();

        var fileMenu = new ButtonMenuItem() { Text = "File" };
        fileMenu.Items.Add(new Command((_, _)=> this.Quit()) { MenuText = "Quit" });
        this.Menu.Items.Add(fileMenu);

        var infoMenu = new ButtonMenuItem() { Text = "Info" };
        infoMenu.Items.Add(new Command((_, _)=> this.ShowAbout()) { MenuText = "About this program" });
        infoMenu.Items.Add(new Command((_, _)=> this.ShowAbout()) { MenuText = "About license" });
        this.Menu.Items.Add(infoMenu);
    }

    public void InitContent()
    {
        var aInput = new TextBox();
        aInput.TextBinding.Bind(this.Calculator, c => c.A);

        var bInput = new TextBox();
        bInput.TextBinding.Bind(this.Calculator, c => c.B);

        var cInput = new TextBox();
        cInput.TextBinding.Bind(this.Calculator, c => c.C);

        var sOutput = new Label() { VerticalAlignment = VerticalAlignment.Center };

        var calcButton = new Button();
        calcButton.Text = "Calculate";
        calcButton.Click += (_, _) => {
            this.Calculator.Calculate();
            sOutput.Text = this.Calculator.S;
        };

        var layout = new TableLayout() {
            Spacing = new Size(5, 5),
            Padding = new Padding(10, 10, 10, 10)
        };

        layout.Rows.Add (
            new TableRow (new Label() { Text = "A length = ", VerticalAlignment = VerticalAlignment.Center }, aInput)
        );

        layout.Rows.Add (
            new TableRow (new Label() { Text = "B length = ", VerticalAlignment = VerticalAlignment.Center }, bInput)
        );

        layout.Rows.Add (
            new TableRow (new Label() { Text = "C length = ", VerticalAlignment = VerticalAlignment.Center }, cInput)
        );

        layout.Rows.Add (
            new TableRow (new Label(), calcButton)
        );

        layout.Rows.Add (
            new TableRow (new Label() { Text = "Area S = ", VerticalAlignment = VerticalAlignment.Center }, sOutput)
        );

        layout.Rows.Add (
            new TableRow () {
                ScaleHeight = true
            }
        );

        this.Content = layout;
    }

    public void Quit()
    {
        Application.Instance.Quit();
    }

    public void ShowAbout()
    {
        var about = new Form() {
            Title = "About",
            ClientSize = new Size(300, 250),
            MinimumSize = new Size(290, 200),
            Content = new Panel() {
                Padding = new Padding(10, 10, 10, 10),
                Content = new Label() {
                    Text = "Triangle area calculator\nVersion: v0.0.1\nAuthor: Dmytro Naumov 8.1213\nMIT License"
                }
            }
        };

        about.Show();
    }
}
