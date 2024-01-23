using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace JogoWinforms;
public class Jogo : Tela
{
    Button uppop;
    Color?[][] bolas;
    Color?[][] jogadas;
    Color? atual = null;
    List<(int x, int y)> jogadaAtual = new List<(int x, int y)>();
    List<(Point incial, Point final)> linhaTracada = new List<(Point inicial, Point final)>();
    Pontuacao pontuacao = new Pontuacao();
    Tempo tempo = new Tempo();
    FormularioJogo popUp = new FormularioJogo();
    List<(int x, int y)> bolasMarcadas = new List<(int x, int y)>();


    public override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
            Application.Exit();

        if (e.KeyCode == Keys.Space)
            Carregar();
    }

    bool isMouseClicado = false;
    public override void OnMouseMove(MouseEventArgs e)
    {
        if (isMouseClicado)
            this.Jogar((int)e.X, (int)e.Y, PictureBox);
    }

    public override void OnTick()
    {
        Graphics.Clear(Color.Black);
        this.Desenhar(PictureBox, Graphics);
        PictureBox.Refresh();
    }
    Point clicado = Point.Empty; //empty = vazio
    public override void OnMouseDown(MouseEventArgs e)
    {
        clicado = e.Location;
        isMouseClicado = true;
        this.Jogar((int)e.X, (int)e.Y, PictureBox);
    }

    public override void OnMouseUp(MouseEventArgs e)
    {
        clicado = Point.Empty;
        isMouseClicado = false;
        this.ValidarJogada();
    }
    /// <summary>
    /// mudar o tamanho e a quantidade para random
    /// </summary>
    public override void Carregar()
    {
        carregar(15, 20);
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

        if (x < tamX || x > tamX + TamanhoDoQuadrado || y < tamY || y > tamY + TamanhoDoQuadrado)
        {
            return;
        }

        x = (x - tamX) / tamanhoCelula;
        y = (y - tamY) / tamanhoCelula;

        if (x < 0 || x >= bolas.Length || y < 0 || y >= bolas[x].Length)
        {
            return;
        }

        if (atual == null)
        {
            if (bolas[x][y] != null && !bolasMarcadas.Contains((x, y)))
            {
                atual = bolas[x][y];
                jogadaAtual.Add((x, y));
            }
        }
        else
        {
            if (jogadas[x][y] == null)
            {
                jogadas[x][y] = atual;
                jogadaAtual.Add((x, y));
            }

            if (jogadas[x][y] != atual)
            {
                atual = null;
                LimparJogada();
                LimparJogadaAtual();
                return;
            }

            if (bolas[x][y] != atual && bolas[x][y] != null)
            {
                atual = null;
                LimparJogada();
                LimparJogadaAtual();
                return;
            }

        }

    }
    /// <summary>
    /// chama no program, validar coisas da jogada: se foi concluído o trajeto de uma bolinha até a outra sem bater em outra linha traçada
    /// </summary>
    public void ValidarJogada()
    {
        atual = null;
        if (jogadaAtual.Count == 0)
            return;

        var (ultimoX, ultimoY) = jogadaAtual.Last();
        var (primeiroX, primeiroY) = jogadaAtual.First();

        var cor = jogadas[primeiroX][primeiroY];

        if (primeiroX != ultimoX && primeiroY != ultimoY && bolas[primeiroX][primeiroY] == cor && bolas[ultimoX][ultimoY] == cor)
        {
            pontuacao.Pontos += 10;

            foreach (var posicao in jogadaAtual)
            {
                bolasMarcadas.Add(posicao);

            }
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

                if (bolas[i][j] == cor)
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// ideia era remove a última linha traçada se o ponto final não for válido, chama no validaJogada
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ValidarPontoFinal(int x, int y)
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

    /// <summary>
    /// se o movimento de uma bola para outra é válido 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool VerificarMovimento(int x, int y)
    {
        if (jogadaAtual.Count == 0)
            return true;

        var (ultimoX, ultimoY) = jogadaAtual.Last();
        return Math.Abs(x - ultimoX) + Math.Abs(y - ultimoY) == 1 && SemCruzamentos(x, y);
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

    public void DesenharPontuacao(Graphics g)
    {
        string textoPontuacao = "Pontuação: " + pontuacao.Pontos;

        Font fonte = new Font("Arial", 12);
        SolidBrush brush = new SolidBrush(Color.White);

        int posX = PictureBox.Width - 180;
        int posY = 10;
        g.DrawString(textoPontuacao, fonte, brush, posX, posY);

        fonte.Dispose();
        brush.Dispose();
    }
    public void Desenhar(PictureBox pb, Graphics g)
    {
        int TamanhoDoQuadrado = Math.Min(pb.Width, pb.Height) - 300;

        int tamX = (pb.Width - TamanhoDoQuadrado) / 2;
        int tamY = (pb.Height - TamanhoDoQuadrado) / 2;

        // Quadrado 
        g.DrawRectangle(Pens.Black, tamX, tamY, TamanhoDoQuadrado, TamanhoDoQuadrado);

        DesenharBotao();
        int num = 15;

        int tamanhoCelula = TamanhoDoQuadrado / num;

        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                int x = tamX + i * tamanhoCelula;
                int y = tamY + j * tamanhoCelula;

                g.DrawRectangle(Pens.DimGray, x, y, tamanhoCelula, tamanhoCelula);

                if (bolas[i][j] is not null)
                {
                    var cor = bolas[i][j].Value;

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
            DesenharPontuacao(g);
        }



        if (!atual.HasValue) //retorna um valor atual
            return;

        var corJogada = new SolidBrush(atual.Value);

        if (jogadaAtual.Count > 1 && bolas[jogadaAtual[0].x][jogadaAtual[0].y] == atual)
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

    public void DesenharBotao()
    {
        uppop = new Button();
        uppop.FlatStyle = FlatStyle.Flat;
        uppop.FlatAppearance.BorderSize = 0;
        uppop.FlatAppearance.MouseDownBackColor = Color.Transparent;
        uppop.FlatAppearance.MouseOverBackColor = Color.Transparent;
        uppop.BackColor = Color.Black; // Define a cor de fundo inicial do botão
        uppop.ForeColor = Color.White;
        uppop.Font = new Font("Arial", 12);
        uppop.Text = "Menu";
        uppop.Size = new Size(200, 30);
        uppop.Width = 230;
        uppop.Height = 85;
        uppop.Location = new Point(PictureBox.Width - 180, PictureBox.Height - 80);

        uppop.Click += delegate
        {
            popUp.Close();
        };

        uppop.MouseHover += (sender, e) =>
        {
            popUp.ShowDialog();
        };

        PictureBox.Controls.Add(uppop);
    }

    /// <summary>
    /// verifica se ele encaixou o atual com o final
    /// </summary>
    private bool PontoFinalValido(int x, int y)
        => atual != null && jogadas[x][y] == atual;

    private void carregar(int tamanho, int qtdBolas)
    {
        bolas = Enumerable.Range(0, tamanho)
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
            bolas[x][y] = color;

            (x, y) = pegarEspacoVazio(tamanho, alet);
            bolas[x][y] = color;
        }

    }

    /// <summary>
    /// alet aleatorio
    /// </summary>
    private (int x, int y) pegarEspacoVazio(int tamanho, Random alet)
    {
        int x = alet.Next(tamanho);
        int y = alet.Next(tamanho);
        while (bolas[x][y] != null)
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