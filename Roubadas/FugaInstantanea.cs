using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class FugaInstantanea : RoubosJogo
    {
        public FugaInstantanea()
        {
            this.QuantidadeJogadas = 28;
            this.Identificacao = "Fuga Instantanea";
            this.Foto = Image.FromFile("./assets/img/fugainstatanea.png");
        }
    }
}