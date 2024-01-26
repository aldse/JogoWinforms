using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JogoWinforms.Roubadas;

namespace JogoWinforms;

public class MenuDeRoubo : Tela
{
    Jogo fundo = null;
    public MenuDeRoubo(Jogo fundo)
        => this.fundo = fundo;

    private int animation = 0;
    private int espacamentoCards = 20;

    List<RectangleF> cards = new List<RectangleF>();
    int selectedIndex = -1;

    bool isMouseClicado = false;
    private RectangleF cartaEmMovimento = RectangleF.Empty;
    private Point ultimaposicaoMouse = Point.Empty;
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

        foreach (var card in cards)
        {
            g.FillRectangle(Brushes.White, card);
        }

        PictureBox.Refresh();
    }

    public override void Carregar()
    {
        const float propWidth = 0.6f;
        var pb = PictureBox;

        int tamanhoXFinal = (int)(pb.Width * propWidth);
        int restoX = pb.Width - tamanhoXFinal;

        for (int i = 0; i < 5; i++)
        {
            int cardX = restoX / 2 + i * (200 + espacamentoCards) + 37;
            int cardY = 250;
            AddCard(cardX, cardY);
        }
    }

  public override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].Contains(e.Location))
            {
                cartaEmMovimento = cards[i];
                ultimaposicaoMouse = e.Location;
                selectedIndex = i;

                cards.RemoveAt(selectedIndex);

                break;
            }
        }
    }

    public override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);

        if (!cartaEmMovimento.IsEmpty)
        {
            cards.Insert(selectedIndex, cartaEmMovimento);
            cartaEmMovimento = RectangleF.Empty;
            ultimaposicaoMouse = Point.Empty;
        }
    }

    public override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (!cartaEmMovimento.IsEmpty)
        {
            float deltaX = e.Location.X - ultimaposicaoMouse.X;
            float deltaY = e.Location.Y - ultimaposicaoMouse.Y;

            cartaEmMovimento.X += deltaX;
            cartaEmMovimento.Y += deltaY;

            ultimaposicaoMouse = e.Location;

            PictureBox.Invalidate(); 
        }
    }
    /// <summary>
    ///   cardWidth -> Largura do card 
    ///   cardHeight -> Altura do card
    ///   cardX -> Posição X do card branco
    ///   cardY -> Posição Y do card branco
    /// </summary>
    private void AddCard(int cardX, int cardY)
    {
        var g = Graphics;
        var pb = PictureBox;
        int cardWidth = 200;
        int cardHeight = 200;
        this.cards.Add(new RectangleF(cardX, cardY, cardWidth, cardHeight));
    }

}