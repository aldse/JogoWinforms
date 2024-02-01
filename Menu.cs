using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace JogoWinforms
{
    public class Menu : Tela
    {
        private Button ngBtn;
        private Button cntBtn;
        private Label melhorPontuacaoLabel;

        public override void Carregar()
        {
            Pontuacao pontuacao = new Pontuacao();

            Image background = Image.FromFile("img/bom1.png");
            Image jogar = Image.FromFile("img/jogar.png");
            Image sair = Image.FromFile("img/sair.png");

            MainForm.WindowState = FormWindowState.Maximized;
            MainForm.FormBorderStyle = FormBorderStyle.None;
            MainForm.Text = "Joguinho";

            // MessageBox.Show($"{PictureBox.Size}"); 1920x1080
            ngBtn = CriarBotao(jogar, 
                .18f * PictureBox.Width, 
                .32f * PictureBox.Height, 
                .35f * PictureBox.Width,
                .64f * PictureBox.Height
            );
            cntBtn = CriarBotao(sair, 300, 340, 1020, 610);

            melhorPontuacaoLabel = new Label
            {
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(0xBD, 0xD4, 0xC8),
                Font = new Font("Tw Cen MT Condensed Extra Bold", 20, FontStyle.Regular),
                Text = "Melhor Pontuação: " + pontuacao.ObterMaiorPontuacao(),
                AutoSize = true,
                Location = new Point(1650, 40)
            };
            PictureBox.Controls.Add(melhorPontuacaoLabel);

            PictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBox.BackgroundImage = background;
            PictureBox.Controls.Add(ngBtn);
            PictureBox.Controls.Add(cntBtn);
        }

        private Button CriarBotao(Image imagem, float largura, float altura, float x, float y)
        {
            Button botao = new Button
            {
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.Transparent, MouseOverBackColor = Color.Transparent },
                BackgroundImage = imagem,
                BackgroundImageLayout = ImageLayout.Stretch,
                Width = (int)largura,
                Height = (int)altura,
                Location = new Point((int)x, (int)y),
                BackColor = Color.Transparent
            };

            // Arredondar os cantos do botão
            GraphicsPath path = new GraphicsPath();
            int raio = 10; // Ajuste o raio para arredondar mais ou menos
            path.AddArc(0, 0, raio * 2, raio * 2, 180, 90);
            path.AddArc(botao.Width - 2 * raio, 0, raio * 2, raio * 2, 270, 90);
            path.AddArc(botao.Width - 2 * raio, botao.Height - 2 * raio, raio * 2, raio * 2, 0, 90);
            path.AddArc(0, botao.Height - 2 * raio, raio * 2, raio * 2, 90, 90);
            botao.Region = new Region(path);

            botao.MouseEnter += (sender, e) =>
            {
                botao.ForeColor = Color.Black;
            };

            botao.MouseLeave += (sender, e) =>
            {
                botao.ForeColor = Color.White;
            };

            botao.Click += delegate
            {
                Tela tela = new Jogo
                {
                    PictureBox = this.PictureBox,
                    MainForm = this.MainForm,
                    Graphics = this.Graphics
                };
                tela.Carregar();
                Program.AtualizarTela(tela);
                PictureBox.Controls.Clear();
            };

            return botao;
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}
