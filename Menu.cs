using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace JogoWinforms
{
    public class Menu : Tela
    {
        public GameSound gameSound = new GameSound();
        private Button ngBtn;
        private Button cntBtn;
        private Label melhorPontuacaoLabel;

        public override void Carregar()
        {
            gameSound.PlayMusic("./assets/sounds/msc.mp3");

            Pontuacao pontuacao = new Pontuacao();

            Image background = Image.FromFile("./assets/img/bom1.png");
            Image jogar = Image.FromFile("./assets/img/jogar.png");
            Image sair = Image.FromFile("./assets/img/sair.png");

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
            cntBtn = CriarBotao(sair,
                .16f * PictureBox.Width,
                .30f * PictureBox.Height,
                .53f * PictureBox.Width,
                .56f * PictureBox.Height
            );

            cntBtn.Click += (sender, e) =>
            {
                Application.Exit();
            };

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
            Button entrar = new Button
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

            entrar.MouseEnter += (sender, e) =>
            {
                entrar.ForeColor = Color.Black;
            };

            entrar.MouseLeave += (sender, e) =>
            {
                entrar.ForeColor = Color.White;
            };

            entrar.Click += delegate
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

            return entrar;
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}
