using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class RotaLabirintica : RoubosJogo
    {
        public RotaLabirintica()
        {
            this.QuantidadeJogadas = 24;
            this.Identificacao = "Rota Labirintica";
            this.Foto = Image.FromFile("./assets/img/rotalabirintica.png");
        }
    }
}