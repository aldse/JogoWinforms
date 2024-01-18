using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace JogoWinforms;
public class Jogo
{   //? indica que o tipo é anulável 
    Color?[][] dados;
    Color?[][] jogadas;
    Color? atual = null;
    List<(int x, int y)> jogadaAtual = new List<(int x, int y)>();
    List<Point> pontosLinha = new List<Point>();
    List<(Point incial, Point final)> linhaTracada = new List<(Point inicial, Point final)>();

    /// <summary>
    /// mudar o tamanho e a quantidade para random
    /// </summary>
    public void Carregar()
    {
        carregar(15, 10);
    }

    /// <summary>
    /// Coisas que deve ter: vai de um ao outro / não deixa passar aonde ja passou ou aode esta ocupado / cores iguais se encaixam
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pb"></param>
    public void Jogar(int x, int y, PictureBox pb)
    {
        int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 300;

        int tamX = (pb.Width - TamanhoDoQuadrado) / 2;
        int tamY = (pb.Height - TamanhoDoQuadrado) / 2;
        int num = 15;

        int tamanhoCelula = TamanhoDoQuadrado / num;

        x = (x - tamX) / tamanhoCelula;
        y = (y - tamY) / tamanhoCelula;

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
            if (jogadas[x][y] == null && dados[x][y] == null)
            {
                if (jogadaAtual.Count > 1)
                {
                    pontosLinha.AddRange(jogadaAtual.Select(p => new Point(p.x, p.y)));
                    linhaTracada.Add((pontosLinha.First(), pontosLinha.Last()));
                }

                jogadas[x][y] = atual;
                jogadaAtual.Add((x, y));
            }
        }
    }
    public void ValidarJogada() //chama no program, validar coisas da jogada: se foi concluído o trajeto de uma bolinha até a outra sem bater em outra linha traçada
    {
        atual = null;
        if (jogadaAtual.Count == 0)
            return;

        var (ultimoX, ultimoY) = jogadaAtual.Last();
        if (temBolinhaPerto(ultimoX, ultimoY))
        {
            MessageBox.Show("Opa");
            //ele tem que ver se ele encaixou uma na outra sem cruzar ninguem 
        }
        else
        {

            LimparJogada();
        }

        LimparJogadaAtual();
    }

    private void LimparJogada()
    {
        foreach (var jogadaAtualCoordenadas in jogadaAtual)
        {
            int x = jogadaAtualCoordenadas.x;
            int y = jogadaAtualCoordenadas.y;

            if (jogadas[x][y] != null)
            {
                jogadas[x][y] = null;
            }
        }
    }




    /// <summary>
    /// Procura se existe uma bolinha em volta de uma posição (x, y) e retorna
    /// verdadeiro se a jogada na posição (x, y) possui a mesma cor que a da bolinha., ver caminho
    /// </summary>
    private bool temBolinhaPerto(int x, int y)
    {
        var cor = jogadas[x][y];
        var tamanho = jogadas.Length;

        for (int i = x - 1; i < x + 2; i++)
        {
            if (i < 0 || i >= tamanho)
                continue;

            for (int j = y - 1; j < y + 2; j++)
            {
                if (j < 0 || j >= jogadas[i].Length)
                    continue;

                if (x == i && y == j)
                    continue;

                if (dados[i][j] == cor)
                    return true;
            }
        }
        return false;
    }

    public void ValidarPontoFinal(int x, int y) // ideia era remove a última linha traçada se o ponto final não for válido, chama no validaJogada
    {
        if (jogadaAtual.Count > 1)
        {
            var (ultimoX, ultimoY) = jogadaAtual.Last();
            if (!PontoFinalValido(ultimoX, ultimoY))
            {
                linhaTracada.RemoveAt(linhaTracada.Count - 1);
            }
        }
    }
    public bool VerificarMovimento(int x, int y) // se o movimento de uma bola para outra é válido 
    {
        if (jogadaAtual.Count == 0)
            return true;

        var (ultimoX, ultimoY) = jogadaAtual.Last();
        return Math.Abs(x - ultimoX) + Math.Abs(y - ultimoY) == 1 && SemCruzamentos(x, y);
    }
    private bool CruzamComLinhaTraçada(int x, int y)
    {
        foreach (var tracoExistente in linhaTracada)
        {
            if (Cruzam((tracoExistente.incial, tracoExistente.final), (new Point(jogadaAtual.Last().x, jogadaAtual.Last().y), new Point(x, y))))
            {
                return true;
            }
        }
        return false;
    }

    public bool SemCruzamentos(int x, int y)
    {
        if (jogadaAtual.Count == 0)
            return true;

        foreach (var tracoExistente in linhaTracada)
        {
            if (Cruzam((tracoExistente.incial, tracoExistente.final), (new Point(jogadaAtual.Last().x, jogadaAtual.Last().y), new Point(x, y))))
            {
                return true;
            }
        }

        return true;
    }

    private bool Cruzam((Point comeco, Point fim) segmento1, (Point comeco, Point fim) segmento2)
    {
        float a1 = segmento1.fim.Y - segmento1.comeco.Y;
        float b1 = segmento1.comeco.X - segmento1.fim.X;
        float c1 = a1 * segmento1.comeco.X + b1 * segmento1.comeco.Y;

        float a2 = segmento2.fim.Y - segmento2.comeco.Y;
        float b2 = segmento2.comeco.X - segmento2.fim.X;
        float c2 = a2 * segmento2.comeco.X + b2 * segmento2.comeco.Y;

        float delta = a1 * b2 - a2 * b1;

        if (delta == 0)
        {
            return false;
        }

        float intersectX = (b2 * c1 - b1 * c2) / delta;
        float intersectY = (a1 * c2 - a2 * c1) / delta;

        bool dentroSegmento1 = intersectX >= Math.Min(segmento1.comeco.X, segmento1.fim.X)
                            && intersectX <= Math.Max(segmento1.comeco.X, segmento1.fim.X)
                            && intersectY >= Math.Min(segmento1.comeco.Y, segmento1.fim.Y)
                            && intersectY <= Math.Max(segmento1.comeco.Y, segmento1.fim.Y);

        bool dentroSegmento2 = intersectX >= Math.Min(segmento2.comeco.X, segmento2.fim.X)
                            && intersectX <= Math.Max(segmento2.comeco.X, segmento2.fim.X)
                            && intersectY >= Math.Min(segmento2.comeco.Y, segmento2.fim.Y)
                            && intersectY <= Math.Max(segmento2.comeco.Y, segmento2.fim.Y);

        return dentroSegmento1 && dentroSegmento2;
    }

    public void LimparJogadaAtual()
        => jogadaAtual.Clear();

    public void Desenhar(PictureBox pb, Graphics g)
    {
        int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 300;

        int tamX = (pb.Width - TamanhoDoQuadrado) / 2;
        int tamY = (pb.Height - TamanhoDoQuadrado) / 2;

        // Quadrado 
        g.DrawRectangle(Pens.Black, tamX, tamY, TamanhoDoQuadrado, TamanhoDoQuadrado);

        int num = 15;

        int tamanhoCelula = TamanhoDoQuadrado / num;

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

        if (!atual.HasValue) //retorna um valor atual
            return;

        var corJogada = new SolidBrush(atual.Value);

        if (jogadaAtual.Count > 1 && dados[jogadaAtual[0].x][jogadaAtual[0].y] == atual)
        {
            foreach (var jogada in jogadaAtual)
            {
                g.FillRectangle(corJogada,
                    tamX + jogada.x * tamanhoCelula,
                    tamY + jogada.y * tamanhoCelula,
                    tamanhoCelula, tamanhoCelula
                );
            }
        }
    }


    /// <summary>
    /// verifica se ele encaixou o atual com o final
    /// </summary>
    private bool PontoFinalValido(int x, int y)
        => atual != null && jogadas[x][y] == atual;

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

    /// <summary>
    /// alet aleatorio
    /// </summary>
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