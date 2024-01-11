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

            g.DrawString("Jogo Winforms", form.Font, Brushes.WhiteSmoke, Point.Empty);
            
            int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 500;

            int tamX = (pb.Width - TamanhoDoQuadrado) / 2;
            int tamY = (pb.Height - TamanhoDoQuadrado) / 2;

            // Quadrado 
            g.DrawRectangle(Pens.Black, tamX, tamY, TamanhoDoQuadrado, TamanhoDoQuadrado);

            Random randnum = new Random();
            int num = randnum.Next(2, 40);

            int tamanhoCelula = TamanhoDoQuadrado / num;

            // Grade aleatória
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    int x = tamX + i * tamanhoCelula;
                    int y = tamY + j * tamanhoCelula;

                    g.DrawRectangle(Pens.YellowGreen, x, y, tamanhoCelula, tamanhoCelula);
                }
            }

            pb.Image = bmp;
        };

        Application.Run(form);
    }
}
