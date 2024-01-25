using System.Drawing;

namespace JogoWinforms;

public class MenuDeRoubo : Tela
{
    Jogo fundo = null;
    public MenuDeRoubo(Jogo fundo)
        => this.fundo = fundo;

    private int animation = 0;

    public override void OnTick()
    {
        const int duration = 10;
        const float propWidth = 0.6f;
        const float propHeight = 0.6f;
        var g = Graphics;
        var pb = PictureBox;

        g.Clear(Color.Black);
        fundo.Desenhar(PictureBox, Graphics);

        int tamanhoXFinal = (int)(pb.Width * propWidth);
        int tamanhoYFinal = (int)(pb.Height * propHeight);
        int restoX = pb.Width - tamanhoXFinal;
        int restoY = pb.Height - tamanhoYFinal;
        int moveX = (pb.Width - restoX / 2) / duration;
        int moveY = (pb.Height - restoY / 2) / duration;
        int moveX2 = restoX / 2 / duration;
        int moveY2 = restoY / 2 / duration;

        if (animation < duration)
        {
            animation++;

            g.FillRectangle(Brushes.SkyBlue, 
                pb.Width - moveX * animation,
                pb.Height - moveY * animation, 
                (moveX - moveX2) * animation, 
                (moveY - moveY2) * animation
            );
            PictureBox.Refresh();
            return;
        }

        g.FillRectangle(Brushes.SkyBlue, 
            pb.Width - moveX * duration,
            pb.Height - moveY * duration, 
            (moveX - moveX2) * duration, 
            (moveY - moveY2) * duration
        );

        // AQUI

        PictureBox.Refresh();
    }
}