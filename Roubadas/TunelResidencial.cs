using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class TunelResidencial : RoubosJogo
    {
        public TunelResidencial()
        {
            this.QuantidadeJogadas = 20;
            this.Identificacao = "Tunel Residencial";
            this.Foto = Image.FromFile("./assets/img/tunelresidencial.png");
        }
    }
}