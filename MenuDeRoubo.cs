using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JogoWinforms.Roubadas;

using System;
using System.Drawing.Drawing2D;

namespace JogoWinforms;

public class MenuDeRoubo : Tela
{
    Image background = Image.FromFile("img/talvez.png");
    Jogo fundo = null;
    List<PictureBox> rouboCards = new List<PictureBox>();
    public MenuDeRoubo(Jogo fundo)
        => this.fundo = fundo;

    private int animation = 0;
    List<RoubosJogo> roubos = new List<RoubosJogo>();
    RoubosJogo[] vetorRooubos = new RoubosJogo[10];
    int selectedIndex = -1;
    private RoubosJogo cartaEmMovimento = null;
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
            using (var backgroundImageBrush = new TextureBrush(background))
            {
                g.FillRectangle(backgroundImageBrush,
                    pb.Width - moveX * animation,
                    pb.Height - moveY * animation,
                    (moveX - moveX2) * animation,
                    (moveY - moveY2) * animation
                );
            }
            PictureBox.Refresh();
            return;
        }

        using (var backgroundImageBrush = new TextureBrush(background))
        {
            g.FillRectangle(backgroundImageBrush,
                pb.Width - moveX * duration,
                pb.Height - moveY * duration,
                (moveX - moveX2) * duration,
                (moveY - moveY2) * duration
            );
        }

        foreach (var roubo in roubos)
        {
            roubo.Desenhar(g);
        }

        if (cartaEmMovimento is not null)
            cartaEmMovimento.Desenhar(g);

        PictureBox.Refresh();
    }

    public override void Carregar()
    {
        const float propWidth = 0.6f;
        var pb = PictureBox;

        int tamanhoXFinal = (int)(pb.Width * propWidth);
        int restoX = pb.Width - tamanhoXFinal;
        int cardX = restoX / 2 + 37;
        int cardYTop = 250;

        List<RoubosJogo> tiposDeRoubos = new List<RoubosJogo>
    {
        new PistaFugaz(),
        new TravessiaDupla(),
        new InversaodeDestino(),
        new TunelResidencial(),
        new RotaLabirintica(),
        new FugaInstantanea(),
        new DancadasBolinhas(),
        new AniquilacaoTatica(),
        new Imigrante(),
        new LinhaInvisivel()
    };

        for (int i = 0; i < 5; i++)
        {
            AddCard(cardX + i * 220, cardYTop, tiposDeRoubos[i]); // Linha superior
            AddCard(cardX + i * 220, cardYTop + 220, tiposDeRoubos[i + 5]); // Linha inferior
        }

        for (int i = 0; i < 10; i++)
        {
            RouboCard(100 + i * 220, 500);
        }
    }

    public override void OnMouseDown(MouseEventArgs e)
    {
        for (int i = 0; i < roubos.Count; i++)
        {
            if (roubos[i].Rectangle.Contains(e.Location))
            {
                cartaEmMovimento = roubos[i];
                ultimaposicaoMouse = e.Location;
                selectedIndex = i;
                break;
            }
        }
    }

    public override void OnMouseUp(MouseEventArgs e)
    {
        if (cartaEmMovimento is null)
            return;

        cartaEmMovimento = null;
        ultimaposicaoMouse = Point.Empty;
    }

    public override void OnMouseMove(MouseEventArgs e)
    {
        float deltaX = e.Location.X - ultimaposicaoMouse.X;
        float deltaY = e.Location.Y - ultimaposicaoMouse.Y;

        ultimaposicaoMouse = e.Location;

        if (cartaEmMovimento is null)
            return;

        cartaEmMovimento.Rectangle = new RectangleF(
            cartaEmMovimento.Rectangle.X + deltaX,
            cartaEmMovimento.Rectangle.Y + deltaY,
            cartaEmMovimento.Rectangle.Width,
            cartaEmMovimento.Rectangle.Height
        );

    }
    /// <summary>
    ///   cardWidth -> Largura do card 
    ///   cardHeight -> Altura do card
    ///   cardX -> Posição X do card branco
    ///   cardY -> Posição Y do card branco
    /// </summary>
    private void AddCard(int cardX, int cardY, RoubosJogo roubo)
    {
        var g = Graphics;
        var pb = PictureBox;
        int cardWidth = 200;
        int cardHeight = 200;
        roubo.Rectangle = new RectangleF(cardX, cardY, cardWidth, cardHeight);
        this.roubos.Add(roubo);
    }
     private void RouboCard(int cardX, int cardY)
        {
            var rouboCard = new PictureBox();
            rouboCard.BackColor = Color.White;
            rouboCard.Size = new Size(200, 200);
            rouboCard.Location = new Point(cardX, cardY);
            rouboCard.BorderStyle = BorderStyle.FixedSingle;
            rouboCards.Add(rouboCard);
        }
}