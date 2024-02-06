using System;
using System.Formats.Tar;
using System.IO;

namespace JogoWinforms
{
    public class Pontuacao
    {
        const string txt = "./melhorpontuacao.txt";
        private int pontos = 0;
        public int Pontos
        {
            get
            {
                return pontos;
            }
            set
            {
                pontos = value;
                ChecarPontuacao();
            }
        }
        private int maiorPontuacao;

        public Pontuacao() {
            CarregarMelhorPontuacao();
        }

        public void Comecar()
        {
            Pontos = 0;
            CarregarMelhorPontuacao();
        }
        private void ChecarPontuacao()
        {
            if (pontos > maiorPontuacao)
            {
                maiorPontuacao = pontos;
                SalvarMelhorPontuacao();
            }
        }
        public int ObterMaiorPontuacao()
        {
            return maiorPontuacao;
        }

        public void CarregarMelhorPontuacao()
        {
            string diretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoArquivo = Path.Combine(diretorio, txt);
            maiorPontuacao = Convert.ToInt32(File.ReadAllText(caminhoArquivo));
        }

        public void SalvarMelhorPontuacao()
        {
            string diretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoArquivo = Path.Combine(diretorio, txt);
            File.WriteAllText(caminhoArquivo, maiorPontuacao.ToString());
        }
    }
}
