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

    public void Carregar()
    {
        carregar(15, 10);
    }

    public void Jogar(int x, int y)
    {
        //vai de um ao outro
        //não deixa passar aonde ja passou ou aode esta ocupado
        //cores iguais se encaixam
    }

    public void ValidarJogada()
    {

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