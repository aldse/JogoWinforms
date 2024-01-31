using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class PistaFugaz : RoubosJogo
    {
        public PistaFugaz()
        {
            this.QuantidadeJogadas = 1;
            this.Identificacao = "Pista Fugaz";
            this.Foto = Image.FromFile("img/pistafugaz2.png");
        }
    }
}