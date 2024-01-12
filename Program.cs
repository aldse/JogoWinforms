using System;
using System.Drawing;
using System.Windows.Forms;
using JogoWinforms;
class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Jogo jogo = new Jogo();
        Bitmap bmp = null;
        Graphics g = null;

        Point clicado = Point.Empty;
        bool isMouseDown = new Boolean();

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
            g.Clear(Color.Black);
            g.DrawString("Jogo Winforms", form.Font, Brushes.WhiteSmoke, Point.Empty);
            jogo.Desenhar(pb, g);
            pb.Refresh();
        };

        form.MouseDown += (o, e) =>
        {
            //deve chamar o com Jogar() e o jogo.Desenhar();
            clicado = e.Location;
            isMouseDown = true;
            jogo.Jogar((int)clicado.X, (int)clicado.Y);
            jogo.Desenhar(pb, g);
        };

        form.MouseUp += (o, e) =>
        {
            //com ValidarJogada()
            jogo.ValidarJogada();
        };

        form.MouseMove += (o, e) =>
        {
            if (isMouseDown && clicado != Point.Empty)
            {
                // Verifique se há uma jogada válida antes de executar
                if (jogo.VerificarMovimento((int)e.X, (int)e.Y))
                {
                    jogo.Jogar((int)e.X, (int)e.Y);

                    // Atualize a exibição chamando o método Desenhar
                    jogo.Desenhar(pb, g);
                    pb.Refresh();
                }
            }
        };



        form.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();

            if (e.KeyCode == Keys.Space)
                jogo.Carregar();
        };

        form.Load += (o, e) =>
        {
            bmp = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(bmp);

            jogo.Carregar();

            pb.Image = bmp;
            timer.Start();
        };

        Application.Run(form);
    }
}
