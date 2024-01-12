using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime;
using System.Collections.Generic;

namespace JogoWinforms;
public class Jogo
{
    Color?[][] dados;
    Color?[][] jogadas;
    Color? atual = null;
    List<(int x, int y)> jogadaAtual = new List<(int x, int y)>();

    List<Point> pontosLinha = new List<Point>();
    public void Carregar()
    {
        carregar(15, 10);
    }

    public void Jogar(int x, int y)
    {
        //vai de um ao outro
        //não deixa passar aonde ja passou ou aode esta ocupado
        //cores iguais se encaixam
        if (atual == null)
        {
            if (jogadas[x][y] != null)
            {
                atual = jogadas[x][y];
                jogadaAtual.Add((x, y));
            }
        }
       else
    {
        if (jogadas[x][y] == null && dados[x][y] == null && VerificarMovimento(x, y))
        {
            // Adiciona os pontos da linha quando a jogada é válida
            if (jogadaAtual.Count > 1)
            {
                pontosLinha.AddRange(jogadaAtual.Select(p => new Point(p.x, p.y)));
            }

            jogadas[x][y] = atual;
            dados[x][y] = atual;
            atual = null;
            jogadaAtual.Add((x, y));

            // Limpa a jogada atual se a quantidade de elementos for ímpar
            if (jogadaAtual.Count % 2 != 0)
                LimparJogadaAtual();
        }
    }
}
    private void LimparJogadaAtual()
    {
        foreach (var (i, j) in jogadaAtual)
        {
            jogadas[i][j] = null;
        }
        jogadaAtual.Clear();
    }

    public void ValidarJogada()
    {
        //validar coisas da jogada: se foi concluido o trajeto de uma bolinha ate a outra sem bater em outra linha traçada
        pontosLinha.Clear();
    }


    public void Desenhar(PictureBox pb, Graphics g)
    {
        int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 300;

        int tamX = (pb.Width - TamanhoDoQuadrado) / 2;
        int tamY = (pb.Height - TamanhoDoQuadrado) / 2;

        // Quadrado 
        g.DrawRectangle(Pens.Black, tamX, tamY, TamanhoDoQuadrado, TamanhoDoQuadrado);

        int num = 15;

        int tamanhoCelula = TamanhoDoQuadrado / num;

        // Grade aleatória
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                int x = tamX + i * tamanhoCelula;
                int y = tamY + j * tamanhoCelula;

                g.DrawRectangle(Pens.DimGray, x, y, tamanhoCelula, tamanhoCelula);

                if (dados[i][j] is not null)
                {
                    var cor = dados[i][j].Value;

                    g.FillEllipse(new SolidBrush(cor),
                        tamanhoCelula * i + tamX + 10,
                        tamanhoCelula * j + tamY + 10,
                        tamanhoCelula - 20,
                        tamanhoCelula - 20
                    );
                }
                else if (jogadas[i][j] is not null)
                {
                    var cor = jogadas[i][j].Value;

                    g.FillRectangle(new SolidBrush(cor),
                        tamanhoCelula * i + tamX,
                        tamanhoCelula * j + tamY,
                        tamanhoCelula, tamanhoCelula
                    );
                }
            }
        }
        if (pontosLinha.Count > 1)
        {
            g.DrawLines(Pens.Red, pontosLinha.ToArray());
        }
    }

    public bool VerificarMovimento(int x, int y)
    {
        if (jogadaAtual.Count == 0)
            return true;

        var (ultimoX, ultimoY) = jogadaAtual.Last();
        return Math.Abs(x - ultimoX) + Math.Abs(y - ultimoY) == 1;
    }

    private void carregar(int tamanho, int qtdBolas)
    {
        dados = Enumerable.Range(0, tamanho)
            .Select(x => new Color?[tamanho])
            .ToArray();
        jogadas = Enumerable.Range(0, tamanho)
            .Select(x => new Color?[tamanho])
            .ToArray();
        Random alet = new Random();

        for (int i = 0; i < qtdBolas; i++)
        {
            var color = pegarCor(alet);

            (int x, int y) = pegarEspacoVazio(tamanho, alet);
            jogadas[x][y] = color;
            dados[x][y] = color;

            (x, y) = pegarEspacoVazio(tamanho, alet);
            jogadas[x][y] = color;
            dados[x][y] = color;
        }

    }

    private (int x, int y) pegarEspacoVazio(int tamanho, Random alet)
    {
        int x = alet.Next(tamanho);
        int y = alet.Next(tamanho);
        while (dados[x][y] != null)
        {
            x = alet.Next(tamanho);
            y = alet.Next(tamanho);
        }
        return (x, y);
    }

    private Color pegarCor(Random alet)
    {
        var vermelho = alet.Next(255);
        var verde = alet.Next(255);
        var azul = alet.Next(255);
        return Color.FromArgb(vermelho, verde, azul);
    }

}