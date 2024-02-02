using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JogoWinforms.Roubadas;

namespace JogoWinforms;
public class Jogo : Tela
{
    private Random random = new Random();
    MenuDeRoubo roubos = new MenuDeRoubo(null);
    Button uppop;
    Color?[][] bolas;
    Color?[][] jogadas;
    Color? atual = null;
    List<(int x, int y)> jogadaAtual = new List<(int x, int y)>();
    List<(Point incial, Point final)> linhaTracada = new List<(Point inicial, Point final)>();
    Pontuacao pontuacao = new Pontuacao();
    List<(int x, int y)> bolasMarcadas = new List<(int x, int y)>();
    bool executarDancaDasBolinhas = false;
    List<(int x, int y)> caminhosCompletados = new List<(int x, int y)>();

    int xClicado = -1;
    int yClicado = -1;
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
        if (executarDancaDasBolinhas)
        {
            executarDancaDasBolinhas = false;
        }
        this.Desenhar(PictureBox, Graphics);
        PictureBox.Refresh();
    }
    Point clicado = Point.Empty; //empty = vazio
    public override void OnMouseDown(MouseEventArgs e)
    {
        clicado = e.Location;
        isMouseClicado = true;
        this.Jogar((int)e.X, (int)e.Y, PictureBox);

        int AniquilacaoTatica = roubos.Selecionados
                  .Count(x => x is AniquilacaoTatica);

        int TamanhoDoQuadrado = Math.Min(PictureBox.Width, PictureBox.Height) - 300;
        int tamX = (PictureBox.Width - TamanhoDoQuadrado) / 2;
        int tamY = (PictureBox.Height - TamanhoDoQuadrado) / 2;
        int tamanhoCelula = TamanhoDoQuadrado / 15;
        int x = (e.X - tamX) / tamanhoCelula;
        int y = (e.Y - tamY) / tamanhoCelula;

        if (x >= 0 && x < bolas.Length && y >= 0 && y < bolas[x].Length && bolas[x][y] != null)
        {
            if (roubos.Selecionados.Any(x => x is AniquilacaoTatica))
            {
                xClicado = x;
                yClicado = y;
                MessageBox.Show("Você clicou em uma bola!");
            }
            if (roubos.Selecionados.Any(x => x is InversaodeDestino))
            {
                xClicado = x;
                yClicado = y;
                MessageBox.Show("Você clicou em uma bola. Clique novamente para escolher o destino.");
            }
            if (roubos.Selecionados.Any(x => x is DancadasBolinhas))
            {
                xClicado = x;
                yClicado = y;
                DialogResult result = MessageBox.Show("Deseja mover esta bola?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    executarDancaDasBolinhas = true;
                    MoverBolaParaPosicaoAleatoria(x, y);
                }
            }
        }
    }
    public override void OnMouseUp(MouseEventArgs e)
    {
        clicado = Point.Empty;
        isMouseClicado = false;
        this.ValidarJogada();

        if (xClicado != -1 && yClicado != -1)
        {
            int xDestino = (int)e.X;
            int yDestino = (int)e.Y;

            int TamanhoDoQuadrado = Math.Min(PictureBox.Width, PictureBox.Height) - 300;
            int tamX = (PictureBox.Width - TamanhoDoQuadrado) / 2;
            int tamY = (PictureBox.Height - TamanhoDoQuadrado) / 2;
            int tamanhoCelula = TamanhoDoQuadrado / 15;
            xDestino = (xDestino - tamX) / tamanhoCelula;
            yDestino = (yDestino - tamY) / tamanhoCelula;

            if (xDestino >= 0 && xDestino < bolas.Length && yDestino >= 0 && yDestino < bolas[xDestino].Length)
            {
                if (bolas[xDestino][yDestino] == null)
                {
                    bolas[xDestino][yDestino] = bolas[xClicado][yClicado];
                    bolas[xClicado][yClicado] = null;
                    xClicado = -1;
                    yClicado = -1;

                    PictureBox.Refresh();
                }
                else
                {
                    MessageBox.Show("Destino ocupado. Escolha outra posição.");
                }
            }
        }
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

        int AniquilacaoTatica = roubos.Selecionados //FUNCIONA MAS ELE DPS DA PRIMEIRA CLICADA QUALQUER Q PASSA O MOUSE EM CIMA ELE PERGUNTA
            .Count(x => x is AniquilacaoTatica);

        int linhasInvisveis = roubos.Selecionados //FUNCIONA
            .Count(x => x is LinhaInvisivel);

        int rotaLabirinticas = roubos.Selecionados  //AINDA N FUNCIONA
            .Count(x => x is RotaLabirintica);

        int inversaodeDestinos = roubos.Selecionados //FUNCIONA
            .Count(x => x is InversaodeDestino);

        int dancadasBolinhas = roubos.Selecionados //FUNCIONA MAS ELE VAI E VAI PRA CIMA DE UMA LINHA TRAÇADA
               .Count(x => x is DancadasBolinhas);

        int pistaFugaz = roubos.Selecionados
            .Count(x => x is PistaFugaz);

        if (!roubos.Selecionados.Any(x => x is RotaLabirintica))
        {
            if (x < tamX || x > tamX + TamanhoDoQuadrado || y < tamY || y > tamY + TamanhoDoQuadrado)
            {
                return;
            }
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
            if (roubos.Selecionados.Any(x => x is AniquilacaoTatica))
            {
                int xe = (int)clicado.X;
                int ye = (int)clicado.Y;

                if (xe >= tamX && xe < tamX + TamanhoDoQuadrado && ye >= tamY && ye < tamY + TamanhoDoQuadrado)
                {
                    xe = (xe - tamX) / tamanhoCelula;
                    ye = (ye - tamY) / tamanhoCelula;

                    if (xe >= 0 && xe < bolas.Length && ye >= 0 && ye < bolas[xe].Length && bolas[xe][ye] != null)
                    {
                        var corBolinhaClicada = bolas[xe][ye].Value;

                        if (DialogResult.Yes == MessageBox.Show("Tem certeza que deseja apagar?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        {
                            for (int i = 0; i < bolas.Length; i++)
                            {
                                for (int j = 0; j < bolas[i].Length; j++)
                                {
                                    if (bolas[i][j] == corBolinhaClicada)
                                    {
                                        bolas[i][j] = null;
                                        pontuacao.Pontos += 10;
                                    }
                                }
                            }
                            PictureBox.Refresh();
                        }
                    }
                }
                xClicado = -1;
                yClicado = -1;
            }

            if (jogadas[x][y] == null)
            {
                jogadas[x][y] = atual;
                jogadaAtual.Add((x, y));
            }

            if (jogadas[x][y] != atual && !roubos.Selecionados.Any(x => x is LinhaInvisivel))
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

        if (jogadaAtual.Count == 0)
            return;

        var agora = jogadaAtual.First();
        for (int i = 1; i < jogadaAtual.Count; i++)
        {
            var proximo = jogadaAtual[i];
            var dx = agora.x - proximo.x;
            var dy = agora.y - proximo.y;
            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            if (dx + dy > 1)
            {
                if (linhasInvisveis > 0)
                {
                    var medio = jogadas[(agora.x + proximo.x) / 2][(agora.y + proximo.y) / 2];
                    if (medio is not null && (dx == 0 || dy == 0))
                    {
                        agora = proximo;
                        linhasInvisveis--;
                        continue;
                    }
                }
                LimparJogada();
                LimparJogadaAtual();
                atual = null;
                return;
            }
            agora = proximo;
        }
    }
    /// <summary>
    /// chama no program, validar coisas da jogada: se foi concluído o trajeto de uma bolinha até a outra sem bater em outra linha traçada
    /// </summary>
    public void ValidarJogada()
    {
        this.atual = null;
        if (jogadaAtual.Count == 0)
            return;

        var (ultimoX, ultimoY) = jogadaAtual.Last();
        var (primeiroX, primeiroY) = jogadaAtual.First();

        var cor = jogadas[primeiroX][primeiroY];

        if ((primeiroX != ultimoX || primeiroY != ultimoY) && bolas[primeiroX][primeiroY] == cor && bolas[ultimoX][ultimoY] == cor)
        {
            pontuacao.Pontos += 10;

            foreach (var posicao in jogadaAtual)
            {
                bolasMarcadas.Add(posicao);
                caminhosCompletados.Add(posicao);

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
        return Math.Abs(x - ultimoX) + Math.Abs(y - ultimoY) == 1;
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

        if (!atual.HasValue)
            return;

        var corJogada = new SolidBrush(atual.Value);

        if (jogadaAtual?.Count > 1 && bolas[jogadaAtual[0].x][jogadaAtual[0].y] == atual)
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

    public bool DestinoValido(int origemX, int origemY, int destinoX, int destinoY)
    {
        // Verifique se o destino é uma célula vazia
        return bolas[destinoX][destinoY] == null;
    }


    public void DesenharBotao()
    {
        uppop = new Button();
        uppop.FlatStyle = FlatStyle.Flat;
        uppop.FlatAppearance.BorderSize = 0;
        uppop.FlatAppearance.MouseDownBackColor = Color.Transparent;
        uppop.FlatAppearance.MouseOverBackColor = Color.Transparent;
        uppop.BackColor = Color.Black;
        uppop.ForeColor = Color.White;
        uppop.Font = new Font("Arial", 12);
        uppop.Text = "Menu";
        uppop.Size = new Size(200, 30);
        uppop.Width = 230;
        uppop.Height = 85;
        uppop.Location = new Point(PictureBox.Width - 180, PictureBox.Height - 80);

        uppop.MouseHover += (sender, e) =>
        {
            if (this.roubos.Fundo == null)
            {
                this.roubos = new MenuDeRoubo(this);
                this.roubos.PictureBox = this.PictureBox;
                this.roubos.Graphics = this.Graphics;
                this.roubos.Carregar();
            }
            Program.AtualizarTela(this.roubos);
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

            // Verifique se a posição gerada aleatoriamente já está em caminhosCompletados
            // Se estiver, continue gerando novas posições até encontrar uma que não esteja na lista
            while (caminhosCompletados.Contains((x, y)))
            {
                (x, y) = pegarEspacoVazio(tamanho, alet);
            }

            bolas[x][y] = color;

            (x, y) = pegarEspacoVazio(tamanho, alet);

            while (caminhosCompletados.Contains((x, y)))
            {
                (x, y) = pegarEspacoVazio(tamanho, alet);
            }

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
    private void MoverBolaParaPosicaoAleatoria(int x, int y)
    {
        var corBolinhaClicada = bolas[x][y].Value;
        bolas[x][y] = null;

        (int newX, int newY) = EncontrarPosicaoVazia();
        bolas[newX][newY] = corBolinhaClicada;

        PictureBox.Refresh();
    }

    private (int x, int y) EncontrarPosicaoVazia()
    {
        int tamanho = bolas.Length;
        int x, y;

        do
        {
            x = random.Next(tamanho);
            y = random.Next(tamanho);
        } while (bolas[x][y] != null);

        return (x, y);
    }

}