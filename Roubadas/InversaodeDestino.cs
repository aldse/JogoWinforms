using System.Drawing;

namespace JogoWinforms.Roubadas
{
    public class InversaodeDestino : RoubosJogo
    {
        public InversaodeDestino()
        {
            this.QuantidadeJogadas = 11;
            this.Identificacao = "Inversão de Destino";
            this.Foto = Image.FromFile("img/inversaodedestino.png");
        }
    }
}




