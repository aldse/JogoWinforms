using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class TravessiaDupla : RoubosJogo
    {
        public TravessiaDupla()
        {
            this.QuantidadeJogadas = 3;
            this.Identificacao = "Travessia Dupla";
            this.Foto = Image.FromFile("./assets/img/travessiadupla1.png");
        }
    }
}