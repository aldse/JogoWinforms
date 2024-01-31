using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class AniquilacaoTatica : RoubosJogo
    {
        public AniquilacaoTatica()
        {
            this.QuantidadeJogadas = 35;
            this.Identificacao = "AniquilacaoTatica";
            this.Foto = Image.FromFile("img/aniquilacaotatica.png");
        }
    }
}
