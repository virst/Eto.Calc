using Eto.Forms;
using Eto.Drawing;
using System;
using System.Globalization;

public class MyCalc : Form
{
    private readonly TextBox tb;

    private readonly string[][] btns =
    {
        new string[] {"AC","<-","Sqrt","/" },
        new string[] {"7","8","9","*" },
        new string[] {"4","5","6","-" },
        new string[] {"1","2","3","+" },
        new string[] {"0","+/-",".","=" },
    };

    private bool newNum = true;
    private string action = null;
    private string buff = null;

    CultureInfo ci = new CultureInfo("en-US");
    public MyCalc()
    {
        Title = "Eto.Calc";
        ClientSize = new Size(200, 300);
        TableLayout tl;

        Content = new TableLayout(tb = new TextBox() { ReadOnly = true, Text = "0" }, tl = new TableLayout());
        tb.Font = new(tb.Font.Family, 18);
        tb.TextAlignment = TextAlignment.Right;
        tb.Selection = new Range<int>(1, 1);

        foreach (var r in btns)
        {
            var tr = new TableRow { ScaleHeight = true };
            tl.Rows.Add(tr);
            foreach (var b in r)
            {
                var bt = new Button { Text = b };
                bt.Click += Bt_Click;
                var c = new TableCell { ScaleWidth = true, Control = bt };
                tr.Cells.Add(c);
            }
        }
    }

    private void Bt_Click(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn == null) return;
        string bt = btn.Text;
        Console.WriteLine("Click-{0}", bt);
        Console.WriteLine("newNum-{0};action-{1};buff-{2}", newNum, action, buff);

        if (bt.Length == 1 && char.IsDigit(bt[0]))
        {
            if (newNum || tb.Text == "0")
                tb.Text = "";
            tb.Text += bt;
            newNum = false;
            return;
        }

        if (bt == ".")
        {
            if (newNum)
            {
                tb.Text = "0.";
            }
            if (!tb.Text.Contains("."))
            {
                tb.Text += ".";
            }
            newNum = false;
            return;
        }

        if (bt == "=" || bt == "+" || bt == "-" || bt == "*" || bt == "/")
        {
            Calc();
            newNum = true;
            buff = tb.Text;
            if (bt == "=") return;
            action = bt;
            return;
        }

        if (bt == "AC")
        {
            tb.Text = "0";
            newNum = true;
            return;
        }

        if (bt == "+/-")
        {
            tb.Text = (Double.Parse(tb.Text.TrimEnd('.'), ci) * -1).ToString(ci);
            return;
        }

        if (bt == "Sqrt")
        {
            try
            {
                tb.Text = Math.Sqrt(Double.Parse(tb.Text.TrimEnd('.'), ci)).ToString(ci);
            }
            catch
            {
                tb.Text = "Err";
            }
            newNum = true;
            return;
        }

        if (bt == "<-")
        {
            if (tb.Text == "0") return;
            tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
            if (tb.Text.Length == 0) tb.Text = "0";
            return;
        }
    }

    private void Calc()
    {
        if (action == null || buff == null)
            return;


        switch (action)
        {
            case "+":
                tb.Text = (Double.Parse(buff, ci) + Double.Parse(tb.Text.TrimEnd('.'), ci)).ToString(ci);
                break;
            case "-":
                tb.Text = (Double.Parse(buff, ci) - Double.Parse(tb.Text.TrimEnd('.'), ci)).ToString(ci);
                break;
            case "*":
                tb.Text = (Double.Parse(buff, ci) * Double.Parse(tb.Text.TrimEnd('.'), ci)).ToString(ci);
                break;
            case "/":
                tb.Text = (Double.Parse(buff, ci) / Double.Parse(tb.Text.TrimEnd('.'), ci)).ToString(ci);
                break;
        }
    }

    [STAThread]
    static void Main()
    {
        new Application().Run(new MyCalc());
    }
}