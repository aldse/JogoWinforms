using System.Drawing;

public abstract class RoubosJogo
{
    public int QuantidadeJogadas { get; set; }
    public string Identificacao { get; set; }
    public Image Foto { get; set; }
    public RectangleF Rectangle { get; set; }

    public void Desenhar(Graphics g)
    {
        g.DrawImage(Foto, Rectangle);
    }
}
