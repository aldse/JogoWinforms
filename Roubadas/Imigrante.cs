using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class Imigrante : RoubosJogo
    {
        public Imigrante()
        {
            this.QuantidadeJogadas = 39;
            this.Identificacao = "Imigrante";
            this.Foto = Image.FromFile("img/imigrante.png");
        }
    }
}