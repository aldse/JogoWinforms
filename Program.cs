using System;
using System.Drawing;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Bitmap bmp = null;
        Graphics g = null;

        var pb = new PictureBox
        {
            Dock = DockStyle.Fill,
        };

        var timer = new Timer
        {
            Interval = 20,
        };

        var form = new Form
        {
            WindowState = FormWindowState.Maximized,
            FormBorderStyle = FormBorderStyle.None,
            Controls = { pb }
        };

        form.Load += (o, e) =>
        {
            bmp = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            pb.Image = bmp;

            timer.Start();
        };

        timer.Tick += (o, e) =>
        {
            g.Clear(Color.Black);

            g.DrawString("Jogo Winforms", form.Font, Brushes.WhiteSmoke, Point.Empty);

            int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 500;
            int margemCima = 40;
            int margemLados = 100;

            int tamX = (pb.Width - TamanhoDoQuadrado - margemLados) / 2;
            int tamY = (pb.Height - TamanhoDoQuadrado - margemCima) / 2;

            // linhas horizontais da grade
            for (int i = 0; i < 7; i++)
            {
                int y = tamY + i * TamanhoDoQuadrado / 6;
                g.DrawLine(Pens.YellowGreen, tamX, y, tamX + TamanhoDoQuadrado + margemLados, y);
            }

            // linhas verticais da grade
            for (int i = 0; i < 7; i++)
            {
                int x = tamX + i * (TamanhoDoQuadrado + margemLados) / 6;
                g.DrawLine(Pens.YellowGreen, x, tamY, x, tamY + TamanhoDoQuadrado);
            }
            // Desenha marcadores (pontos) no centro de cada quadradinho
            int tamanhoDoPonto = 60;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    int x = tamX + (TamanhoDoQuadrado + margemLados) / 12;
                    int y = tamY + (TamanhoDoQuadrado / 12);

                    g.FillEllipse(Brushes.Red, x - tamanhoDoPonto / 2, y - tamanhoDoPonto / 2, tamanhoDoPonto, tamanhoDoPonto);
                }
            }
            pb.Refresh();
        };

        Application.Run(form);
    }
}
