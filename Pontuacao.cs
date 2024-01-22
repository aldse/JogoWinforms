using System;
using System.IO;

namespace JogoWinforms
{
    public class Pontuacao : Tela
    {
        private int pontoAtual;
        private int maiorPontuacao;

        public void Comecar()
        {
            pontoAtual = 0;
            CarregarMelhorPontuacao(); // Carrega a melhor pontuação ao iniciar
        }

        public void Atualizar(int novaPontuacao)
        {
            pontoAtual = novaPontuacao;
            ChecarPontuacao();
        }

        private void ChecarPontuacao()
        {
            if (pontoAtual > maiorPontuacao)
            {
                maiorPontuacao = pontoAtual;
            }
        }

        public int ObterPontoAtual()
        {
            return pontoAtual;
        }

        public int ObterMaiorPontuacao()
        {
            return maiorPontuacao;
        }

        public void CarregarMelhorPontuacao()
        {
            string txt = "./melhorpontuacao.txt";

            if (File.Exists(txt))
            {
                try
                {
                    maiorPontuacao = Convert.ToInt32(File.ReadAllText(txt));
                }
                catch (Exception ex)
                {
                    // Lida com exceções se houver algum problema na leitura do arquivo
                    Console.WriteLine("Erro ao ler a melhor pontuação: " + ex.Message);
                }
            }
        }

        public void SalvarMelhorPontuacao()
        {
            string diretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoArquivo = Path.Combine(diretorio, "melhorpontuacao.txt");

            try
            {
                File.WriteAllText(caminhoArquivo, maiorPontuacao.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar a melhor pontuação: " + ex.Message);
            }
        }
    }
}
