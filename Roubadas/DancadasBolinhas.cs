using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class DancadasBolinhas : RoubosJogo
    {
        public DancadasBolinhas()
        {
            this.QuantidadeJogadas = 31;
            this.Identificacao = "Dança das Bolinhas";
            this.Foto = Image.FromFile("img/dancadasbolinhas.png");
        }
    }
}

