using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime;
using System.Collections.Generic;

namespace JogoWinforms;
public class Jogo
{   //? indica que o tipo é anulável 
    Color?[][] dados;
    Color?[][] jogadas;
    Color? atual = null;
    List<(int x, int y)> jogadaAtual = new List<(int x, int y)>();

    List<Point> pontosLinha = new List<Point>();
    public void Carregar()
    {
        carregar(15, 10);
    }

    public void Jogar(int x, int y, PictureBox pb)
    {
        int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 300;

        int tamX = (pb.Width - TamanhoDoQuadrado) / 2;
        int tamY = (pb.Height - TamanhoDoQuadrado) / 2;
        int num = 15;

        int tamanhoCelula = TamanhoDoQuadrado / num;

        x = (x - tamX) / tamanhoCelula;
        y = (y - tamY) / tamanhoCelula;
        
        //Coisas que deve ter: vai de um ao outro / não deixa passar aonde ja passou ou aode esta ocupado / cores iguais se encaixam
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
                    pontosLinha.AddRange(jogadaAtual.Select(p => new Point(p.x, p.y)));

                jogadas[x][y] = atual;
                jogadaAtual.Add((x, y));
            }
        }
    }
    private void LimparJogadaAtual()
    {
        jogadaAtual.Clear();
    }

    public void ValidarJogada()
    {
        //validar coisas da jogada: se foi concluido o trajeto de uma bolinha ate a outra sem bater em outra linha traçada
        atual = null;
        LimparJogadaAtual();
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

        // // Pontos
        // foreach (var ponto in pontosLinha)
        // {
        //     int x1 = tamX + ponto.X * tamanhoCelula + tamanhoCelula / 2;
        //     int y1 = tamY + ponto.Y * tamanhoCelula + tamanhoCelula / 2;

        //     if (pontosLinha.Contains(new Point(ponto.X + 1, ponto.Y)))
        //     {
        //         int x2 = x1 + tamanhoCelula;
        //         int y2 = y1;
        //         g.DrawLine(Pens.DimGray, x1, y1, x2, y2);
        //     }

        //     if (pontosLinha.Contains(new Point(ponto.X, ponto.Y + 1)))
        //     {
        //         int x2 = x1;
        //         int y2 = y1 + tamanhoCelula;
        //         g.DrawLine(Pens.DimGray, x1, y1, x2, y2);
        //     }
        // }

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
                        tamanhoCelula,
                        tamanhoCelula
                    );
                }
            }
        }

        if (!atual.HasValue)
            return;
        
        var corJogada = new SolidBrush(atual.Value);
        foreach (var jogada in jogadaAtual)
        {
            g.FillRectangle(corJogada,
                tamX + jogada.x * tamanhoCelula,
                tamY + jogada.y * tamanhoCelula,
                tamanhoCelula, tamanhoCelula
            );
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