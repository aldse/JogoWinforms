using System;
using System.Drawing;
using System.Windows.Forms;
using JogoWinforms;
class Program
{
    [STAThread]
    static void Main()
    {
        Tela atual = null;
        ApplicationConfiguration.Initialize();

        Bitmap bmp = null;
        Graphics g = null;

        Point clicado = Point.Empty; //empty = vazio

        Timer timer = new Timer()
        {
            Interval = 20
        };

        var pb = new PictureBox
        {
            Dock = DockStyle.Fill,
        };

        var form = new Form
        {
            WindowState = FormWindowState.Maximized,
            FormBorderStyle = FormBorderStyle.None,
            Controls = { pb }
        };

        timer.Tick += delegate
        {
            atual.OnTick();
        };

        pb.MouseDown += (o, e) =>
        {
            atual.OnMouseDown(e);
        };

        pb.MouseUp += (o, e) =>
        {
            atual.OnMouseUp(e);
        };

        pb.MouseMove += (o, e) =>
        {
            atual.OnMouseMove(e);
        };

        form.KeyDown += (o, e) =>
        {
            atual.OnKeyDown(e);
        };

        form.Load += (o, e) =>
        {
            atual = new Menu
            {
                MainForm = form,
                Graphics = g,
                PictureBox = pb
            };

            bmp = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(bmp);

            atual.Carregar();

            pb.Image = bmp;
            timer.Start();
        };

        Application.Run(form);
    }
}