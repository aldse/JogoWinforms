using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class LinhaInvisivel : RoubosJogo
    {
        public LinhaInvisivel()
        {
            this.QuantidadeJogadas = 47;
            this.Identificacao = "Linha Invisível";
            this.Foto = Image.FromFile("img/linhainvisivel.png");
        }
    }
}